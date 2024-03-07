using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Profiling;
using System.Text;
using UnityEngine.UI;
using TMPro;
using Unity.Profiling.LowLevel.Unsafe;

public class Debuger : MonoBehaviour
{
    public DebugMode debugMode = DebugMode.MoreDetails;
    [SerializeField] private TMP_Text _text;

    private int _count = 0;
    private float _time = 0;

    private ProfilerRecorder _systemMemoryRec;
    private ProfilerRecorder _mainThreadTotalRec;
    private ProfilerRecorder _mainThreadRec;
    private ProfilerRecorder _presentRec;
    private ProfilerRecorder _renderThreadRec;
    private ProfilerRecorder _gpuRec;
    private ProfilerRecorder _drawCallsRec;
    private ProfilerRecorder _trianglesRec;
    private ProfilerRecorder _verticesRec;
    private ProfilerRecorder _setPassCallsRec;

    private static double GetRecorderFrameAverage(ProfilerRecorder recorder, out float frameDif, out float maxLag)
    {
        frameDif = 0;
        maxLag = 0;
        int samplesCount = recorder.Capacity;
        if (samplesCount == 0)
            return 0;

        double r = 0;
        float[] dif = new float[samplesCount - 1];
        float mdif = 0;
        unsafe
        {
            ProfilerRecorderSample* samples = stackalloc ProfilerRecorderSample[samplesCount];
            recorder.CopyTo(samples, samplesCount);
            for (int i = 0; i < samplesCount; ++i)
                r += samples[i].Value;
            for (int i = 1; i < samplesCount; i++)
                dif[i - 1] = samples[i].Value - samples[i - 1].Value;
            for (int i = 0; i < dif.Length; ++i)
                mdif += dif[i];
            for (int i = 0; i < dif.Length; i++)
                maxLag = Mathf.Max(maxLag, samples[i].Value);
            r /= samplesCount;
            mdif /= dif.Length;
        }
        frameDif = Mathf.Abs(mdif);
        return r;
    }

#if UNITY_EDITOR
   /* private void OnValidate()
    {
        TryGetComponent(out _text);  
    }*/
#endif

    private void OnEnable()
    {
        _systemMemoryRec = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        _mainThreadTotalRec = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "CPU Total Frame Time", 15);
        _mainThreadRec = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "CPU Main Thread Frame Time", 15);
        _presentRec = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "CPU Main Thread Present Wait Time", 15);
        _renderThreadRec = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "CPU Render Thread Frame Time", 15);
        _gpuRec = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "GPU Frame Time", 15);
        _drawCallsRec = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
        _trianglesRec = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
        _verticesRec = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        _setPassCallsRec = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
    }

    private void OnDisable()
    {
        _systemMemoryRec.Dispose();
        _mainThreadTotalRec.Dispose();
        _mainThreadRec.Dispose();
        _presentRec.Dispose();
        _renderThreadRec.Dispose();
        _gpuRec.Dispose();
        _drawCallsRec.Dispose();
        _trianglesRec.Dispose();
        _verticesRec.Dispose();
        _setPassCallsRec.Dispose();
    }

    void Update()
    {
        if (_time >= 1)
        {
            switch (debugMode)
            {
                case DebugMode.Off:
                    _text.text = "";
                    break;
                case DebugMode.FpsOnly:
                    _text.text = $"Fps: {_count}\n";
                    break;
                case DebugMode.Details:
                    double frameTime = GetRecorderFrameAverage(_mainThreadTotalRec, out float diffference, out float lag);
                    _text.text = $"Fps: {_count}\n"
                        + $"System Memory: {_systemMemoryRec.LastValue / (1024 * 1024)} MB\n"
                        + $"Frame Time: {frameTime * (1e-6f):F2} ms\n"
                        + $"Frame difference: {diffference * (1e-6f):F3} ms\n"
                        + $"Maximum lag: {lag * (1e-6f):F3} ms\n";
                    break;
                case DebugMode.MoreDetails:
                    double frameTime2 = GetRecorderFrameAverage(_mainThreadTotalRec, out float diffference2, out float lag2);
                    _text.text = $"Fps: {_count}\n"
                        + $"System Memory: {_systemMemoryRec.LastValue / (1024 * 1024)} MB" + "\n"
                        + $"Total CPU: {frameTime2 * (1e-6f):F2} ms\n"
                        + $"Total difference: {diffference2 * (1e-6f):F3} ms\n"
                        + $"Maximum lag: {lag2 * (1e-6f):F3} ms\n"
                        + $"Main Thread: {GetRecorderFrameAverage(_mainThreadRec, out _, out _) * (1e-6f):F3} ms\n"
                        + $"Main Thread Present Wait: {GetRecorderFrameAverage(_presentRec, out _, out _) * (1e-6f):F3} ms\n"
                        + $"Render Thread: {GetRecorderFrameAverage(_renderThreadRec, out _, out _) * (1e-6f):F3} ms\n"
                        + $"GPU Time: {GetRecorderFrameAverage(_gpuRec, out _, out _) * (1e-6f):F3} ms\n"
                        + $"Draw Calls: {_drawCallsRec.LastValue}\n"
                        + $"SetPass Calls: {_setPassCallsRec.LastValue}\n"
                        + $"Triangles Count: {_trianglesRec.LastValue}\n"
                        + $"Vertices Count: {_verticesRec.LastValue}\n";
                    break;
                default:
                    _text.text = "";
                    break;
            }
            _time = 0;
            _count = 0;
        }
        _time += Time.unscaledDeltaTime;
        _count++;
    }

    public enum DebugMode
    {
        Off,
        FpsOnly,
        Details,
        MoreDetails
    }
}

