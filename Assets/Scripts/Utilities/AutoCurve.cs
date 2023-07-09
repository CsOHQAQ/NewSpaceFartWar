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
    /// 绘制曲线
    /// </summary>
    public void DrawCurve()
    {
        if (_dirty && !ComputeNode())
        {
            return;
        }

        Vector3 start = _nodePath[1].point;
        Vector3 end;

        //绘制节点
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
    /// 通过节点段数的时间大小，获取每段节点
    /// </summary>
    /// <param name="t">时间（0-1）</param>
    /// <returns>计算出的位置</returns>
    public Vector3 GetHermitAtTime(float t)
    {
        if (_dirty && !ComputeNode())
        {
            return new Vector3();
        }

        //最后一个顶点
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
    /// 计算节点。节点数目至少要有2个。
    /// </summary>
    /// <returns>是否成功计算了节点</returns>
    private bool ComputeNode()
    {
        //将节点添加入链表
        if (_points == null || _points.Length < 2)
        {
            //Debug.LogError("节点数目不足或未初始化");
            return false;
        }

        _dirty = false;
        //非闭合曲线与闭合曲线的顶点时间计算：非闭合曲线，最后个顶点的时间为1.0，闭合曲线的最后个顶点的时间为倒数第二个顶点，因为他的最后个点是原点。
        float t = 1f / (_points.Length - 1);
        _nodePath = new List<NodePath>();
        for (int i = 0; i < _points.Length; i++)//根节点不参与路径节点的计算
        {
            _nodePath.Add(new NodePath(_points[i], i * t));
        }
        //非闭合曲线完整的节点
        _nodePath.Insert(0, _nodePath[0]);
        _nodePath.Add(_nodePath[_nodePath.Count - 1]);
        return true;
    }

    /// <summary>
    /// Herimite曲线：获取节点
    /// </summary>
    /// <param name="index">节点最近的顶点</param>
    /// <param name="t"></param>
    /// <returns></returns>
    private Vector3 GetHermitNode(int index, float t)
    {
        Vector3 v;
        Vector3 P0 = _nodePath[index - 1].point;
        Vector3 P1 = _nodePath[index].point;
        Vector3 P2 = _nodePath[index + 1].point;
        Vector3 P3 = _nodePath[index + 2].point;

        //调和函数
        float h1 = 2 * t * t * t - 3 * t * t + 1;
        float h2 = -2 * t * t * t + 3 * t * t;
        float h3 = t * t * t - 2 * t * t + t;
        float h4 = t * t * t - t * t;

        v = h1 * P1 + h2 * P2 + h3 * (P2 - P0) / _smoothFactor + h4 * (P3 - P1) / _smoothFactor;
        return v;
    }

    /// <summary>
    /// 节点类
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