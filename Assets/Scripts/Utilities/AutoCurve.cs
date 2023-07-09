using System.Collections.Generic;
using UnityEngine;

public class AutoCurve
{
    private Vector3[] _points;
    private int _lineCount;
    private float _smoothFactor = 2.0f;
    private bool _dirty;
    private List<NodePath> _nodePath;

    public float SmoothFactor
    {
        get
        {
            return _smoothFactor;
        }
        set
        {
            if (_smoothFactor != value)
            {
                _smoothFactor = value;
                _dirty = true;
            }
        }
    }

    public int LineCount
    {
        get
        {
            return _lineCount;
        }
        set
        {
            if (_lineCount != value)
            {
                _lineCount = value;
                _dirty = true;
            }
        }
    }

    public Vector3[] Point
    {
        get
        {
            return _points;
        }
        set
        {
            _points = value;
            _dirty = true;
        }
    }

    public AutoCurve(int _lineCount, Vector3[] _points = null)
    {
        this._lineCount = _lineCount;
        this._points = _points;
        this._dirty = true;
        ComputeNode();
    }

    /// <summary>
    /// ��������
    /// </summary>
    public void DrawCurve()
    {
        if (_dirty && !ComputeNode())
        {
            return;
        }

        Vector3 start = _nodePath[1].point;
        Vector3 end;

        //���ƽڵ�
        for (int i = 1; i < _lineCount; i++)
        {
            float time = i / (float)_lineCount;
            end = GetHermitAtTime(time);
            Debug.DrawLine(start, end, Color.red);
            start = end;
        }

        Debug.DrawLine(start, _nodePath[_nodePath.Count - 2].point, Color.red);
    }

    /// <summary>
    /// ͨ���ڵ������ʱ���С����ȡÿ�νڵ�
    /// </summary>
    /// <param name="t">ʱ�䣨0-1��</param>
    /// <returns>�������λ��</returns>
    public Vector3 GetHermitAtTime(float t)
    {
        if (_dirty && !ComputeNode())
        {
            return new Vector3();
        }

        //���һ������
        if (t >= _nodePath[_nodePath.Count - 2].time)
        {
            return _nodePath[_nodePath.Count - 2].point;
        }

        int k;
        for (k = 1; k < _nodePath.Count - 2; k++)
        {
            if (_nodePath[k].time > t)
                break;
        }
        if (k > 1)
        {
            k--;
        }
        float param = (t - _nodePath[k].time) / (_nodePath[k + 1].time - _nodePath[k].time);
        return GetHermitNode(k, param);
    }

    /// <summary>
    /// ����ڵ㡣�ڵ���Ŀ����Ҫ��2����
    /// </summary>
    /// <returns>�Ƿ�ɹ������˽ڵ�</returns>
    private bool ComputeNode()
    {
        //���ڵ����������
        if (_points == null || _points.Length < 2)
        {
            //Debug.LogError("�ڵ���Ŀ�����δ��ʼ��");
            return false;
        }

        _dirty = false;
        //�Ǳպ�������պ����ߵĶ���ʱ����㣺�Ǳպ����ߣ����������ʱ��Ϊ1.0���պ����ߵ����������ʱ��Ϊ�����ڶ������㣬��Ϊ������������ԭ�㡣
        float t = 1f / (_points.Length - 1);
        _nodePath = new List<NodePath>();
        for (int i = 0; i < _points.Length; i++)//���ڵ㲻����·���ڵ�ļ���
        {
            _nodePath.Add(new NodePath(_points[i], i * t));
        }
        //�Ǳպ����������Ľڵ�
        _nodePath.Insert(0, _nodePath[0]);
        _nodePath.Add(_nodePath[_nodePath.Count - 1]);
        return true;
    }

    /// <summary>
    /// Herimite���ߣ���ȡ�ڵ�
    /// </summary>
    /// <param name="index">�ڵ�����Ķ���</param>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector3 GetHermitNode(int index, float t)
    {
        Vector3 v;
        Vector3 P0 = _nodePath[index - 1].point;
        Vector3 P1 = _nodePath[index].point;
        Vector3 P2 = _nodePath[index + 1].point;
        Vector3 P3 = _nodePath[index + 2].point;

        //���ͺ���
        float h1 = 2 * t * t * t - 3 * t * t + 1;
        float h2 = -2 * t * t * t + 3 * t * t;
        float h3 = t * t * t - 2 * t * t + t;
        float h4 = t * t * t - t * t;

        v = h1 * P1 + h2 * P2 + h3 * (P2 - P0) / _smoothFactor + h4 * (P3 - P1) / _smoothFactor;
        return v;
    }

    /// <summary>
    /// �ڵ���
    /// </summary>
    private class NodePath
    {
        public Vector3 point;
        public float time;

        public NodePath(Vector3 v, float t)
        {
            point = v;
            time = t;
        }

        public NodePath(NodePath n)
        {
            point = n.point;
            time = n.time;
        }
    }
}