#if UNITY_VERSION < 450
    #include "iPhone_View.h"
#endif

#include <AVFoundation/AVFoundation.h>
#import <UIKit/UIKit.h>
#import <Speech/Speech.h>

#include <stdlib.h>
#include <string.h>
#include <stdint.h>

@interface STTSiOSInterface: NSObject<SFSpeechRecognizerDelegate>
{
    // For STT
    SFSpeechRecognizer* speechRecognizer;
    SFSpeechAudioBufferRecognitionRequest* recognitionRequest;
    SFSpeechRecognitionTask* recognitionTask;
    
    AVAudioEngine* audioEngine;

    // For TTS
    AVSpeechSynthesizer *synthesizer;
}
- (void) textToSpeech: (NSString*)lang speechText:(NSString*)textStr speedRate:(float)speed pitch:(float)pitch;

- (void) speechToText;
- (void) StartRecording;
- (void) StopSTT;
- (void) ReleaseSTTS;

- (void) textToSpeech;

@end

@implementation STTSiOSInterface

typedef void ( *STTCallbackListener ) (const char*);
typedef void ( *ErrorCallbackListener ) (const char*);
static STTCallbackListener g_sttCallbackListener = NULL;
static ErrorCallbackListener g_errorCallbackListener = NULL;

- (void) initSTTS:(NSString*)lang
{
    NSLog(@"Initialize STT: %@", lang);

    // For STT
    speechRecognizer = [[SFSpeechRecognizer alloc]initWithLocale:[[NSLocale alloc] initWithLocaleIdentifier:lang]];
    speechRecognizer.delegate = self;
    
    audioEngine = [[AVAudioEngine alloc] init];

    // For TTS
    synthesizer = [[AVSpeechSynthesizer alloc] init];
}

- (void) speechToText
{
    if (audioEngine.isRunning)
    {
        [audioEngine stop];             // Stop Listening
        [recognitionRequest endAudio];  // Stop Recognition of audio
    } 
    else 
    {
        [self StartRecording];
    }
}

// STT
- (void) StartRecording {
    if (recognitionTask != nil) {
        [recognitionTask cancel];
        recognitionTask = nil;
    }
    
    // Starts an Audio Session
    NSError* error;
    AVAudioSession* audioSession = [AVAudioSession sharedInstance];
    [audioSession setCategory:AVAudioSessionCategoryRecord error:&error];
    [audioSession setActive:YES withOptions:AVAudioSessionSetActiveOptionNotifyOthersOnDeactivation error:&error];
    
    // Starts a recognition process
    recognitionRequest = [[SFSpeechAudioBufferRecognitionRequest alloc] init];
    AVAudioInputNode* inputNode = audioEngine.inputNode;
    recognitionRequest.shouldReportPartialResults = YES;        // After seeing the result of the user's partial recognition
    
    recognitionTask = [speechRecognizer recognitionTaskWithRequest:recognitionRequest resultHandler:^(SFSpeechRecognitionResult * _Nullable result, NSError * _Nullable error) {
        BOOL isFinal = NO;
        if (result) {
            // Whatever you say in the microphone after pressing the button should be being logged in the console
            NSString* sttText = result.bestTranscription.formattedString;
            NSLog(@"STT Result: %@", sttText);

            const char *sttTxtptr = [sttText cStringUsingEncoding:NSUTF8StringEncoding];
            g_sttCallbackListener(sttTxtptr);
            
            isFinal = !result.isFinal;
        }
        if (error) {
            NSString* errorDesc = [error description];
            NSLog(@"STT Error: %@", errorDesc);
            
            const char *errorDescptr = [errorDesc cStringUsingEncoding:NSUTF8StringEncoding];
            g_errorCallbackListener(errorDescptr);

            [audioEngine stop];
            [inputNode removeTapOnBus:0];
            recognitionRequest = nil;
            recognitionTask = nil;
        }
    }];
    
    // Sets the Recording Format
    AVAudioFormat *recordingFormat = [inputNode outputFormatForBus:0];
    [inputNode installTapOnBus:0 bufferSize:1024 format:recordingFormat block:^(AVAudioPCMBuffer * _Nonnull buffer, AVAudioTime * _Nonnull when) {
        [recognitionRequest appendAudioPCMBuffer:buffer];
    }];
    
    // Starts the audio engine,
    [audioEngine prepare];
    [audioEngine startAndReturnError:&error];
    NSLog(@"Start STT");
}

- (void) StopSTT
{
    if (audioEngine != nil)
    {
        [audioEngine stop];
        [audioEngine.inputNode removeTapOnBus:0];
    }
    
    if (recognitionRequest != nil)
        [recognitionRequest endAudio];
    recognitionRequest = nil;
    
    if (recognitionTask != nil)
        [recognitionTask cancel];
    recognitionTask = nil;
}

- (void) ReleaseSTTS
{
    audioEngine = nil;
    recognitionRequest = nil;
    recognitionTask = nil;
}

// TTS
- (void) textToSpeech: (NSString*)lang speechText:(NSString*)textStr speedRate:(float)speed pitch:(float)pitch
{
    NSError* error;
    [[AVAudioSession sharedInstance]setCategory:AVAudioSessionCategoryPlayback error:&error];
    
    if (AVSpeechUtteranceMinimumSpeechRate > speed)
    {
        speed = AVSpeechUtteranceMinimumSpeechRate;
    }
    else if (speed > AVSpeechUtteranceMaximumSpeechRate)
    {
        speed = AVSpeechUtteranceMaximumSpeechRate;
    }

    AVSpeechUtterance *utterance = [AVSpeechUtterance speechUtteranceWithString:textStr];
    utterance.voice = [AVSpeechSynthesisVoice voiceWithLanguage:lang];
    utterance.rate = speed;
    utterance.pitchMultiplier = pitch;
    [synthesizer speakUtterance:utterance];
}

@end


/*
Interface with Unity
*/
static STTSiOSInterface* _GetInterface()
{
    static STTSiOSInterface* _interfaceInstance = nil;
    if (!_interfaceInstance)
    {
        _interfaceInstance = [[STTSiOSInterface alloc] init];
    }
    return _interfaceInstance;
}

static NSString* _GetString(const char* charTxt)
{
    NSString* textStr = nil;
    textStr = [NSString stringWithUTF8String: charTxt];
    return textStr;
}

extern "C" void STTS_Init(const char* lang)
{
    NSString* langType = _GetString(lang);
    [_GetInterface() initSTTS: langType];
}

extern "C" void STTS_StartSTT()
{
    [_GetInterface() speechToText];
}

extern "C" void STTS_StopSTT()
{
    [_GetInterface() StopSTT];
}

extern "C" void STTS_ReleaseSTTS()
{
    [_GetInterface() ReleaseSTTS];
}

extern "C" void RegisterSTTSCallback(STTCallbackListener listener)
{
    g_sttCallbackListener = listener;
}

extern "C" void RegisterErrorCallback(ErrorCallbackListener listener)
{
    g_errorCallbackListener = listener;
}

extern "C" void STTS_StartTTS(const char* lang, const char* textString, float speed, float pitch)
{
    [_GetInterface() textToSpeech: _GetString(lang) speechText:_GetString(textString) speedRate:speed pitch:pitch];
}
