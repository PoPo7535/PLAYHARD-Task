using System;
using UnityEngine;
using System.Collections;

[RequireComponent (typeof(ParticleSystem))]
public class LineParticle : MonoBehaviour
{

	#region Properties

	[SerializeField]
	[Range (10, 1000)]
	private int m_Resolution = 10;
	[SerializeField]
	private float m_Size = 0.25f;
	[SerializeField]
	private Gradient m_ColorLine;
	[SerializeField]
	private FunctionDrawOption m_DrawOption;
	[SerializeField]
	private FunctionColorOption m_ColorOption;
	[SerializeField]
	private Vector3[] m_Points;

	private int _mCurrentPointLength = 0;
	private int _mCurrentResolution = 0;
	private ParticleSystem _mParticleSystem;
	private ParticleSystemRenderer _mParticleSystemRenderer;
	private ParticleSystem.Particle[] _mParticlePoints;
	private static Transform _mTransform;

	private enum FunctionDrawOption
	{
		Linear,
		Sine
	}

	private enum FunctionColorOption
	{
		Static,
		Dynamic
	}

	private delegate Vector3 FunctionDrawDelegate (Vector3 point,Vector3 direction,int index);

	private delegate float FunctionColorDelegate (int index,int length);

	private static readonly FunctionDrawDelegate[] MFunctionDrawDelegates = {
		Linear,
		Sine
	};

	private static readonly FunctionColorDelegate[] MFunctionColorDelegates = {
		Static,
		Dynamic
	};

	#endregion

	#region MonoBehaviour

	private void OnEnable ()
	{
		if (_mParticleSystem != null) {
			DrawPoint ();
		}
	}

	private void OnDisable ()
	{
		if (_mParticleSystem != null) {
			_mParticleSystem.Stop ();
		}
	}

	[Obsolete("Obsolete")]
	private void Awake ()
	{
		_mTransform = this.GetComponent<Transform> ();
		_mParticleSystem = this.GetComponent<ParticleSystem> ();
		_mParticleSystemRenderer = this.GetComponent<ParticleSystemRenderer> ();
		_mParticleSystem.loop = false;
		if (_mParticleSystem.duration > 0.1f) {
			Debug.LogError ("Particle System duration may not work, DURATION must set zero.");
		}
		_mParticleSystem.scalingMode = ParticleSystemScalingMode.Shape;
		_mParticleSystem.playOnAwake = false;
		_mTransform.rotation = Quaternion.identity;
		CreatePoints ();
	}

	private void Update ()
	{
		if (_mCurrentResolution != m_Resolution || _mParticlePoints == null || _mCurrentPointLength != m_Points.Length) {
			CreatePoints ();
		}

		if (_mParticlePoints != null) _mParticleSystem.SetParticles(_mParticlePoints, _mParticlePoints.Length);
		DrawPoint ();
	}

	#endregion

	#region Methods

	public void CreatePoints ()
	{
		if (m_Points.Length == 0) {
			m_Points = new Vector3[]{ Vector3.zero };
		}
		_mCurrentPointLength = m_Points.Length;
		_mCurrentResolution = m_Resolution;
		_mParticlePoints = new ParticleSystem.Particle[m_Resolution * (m_Points.Length - 1)];
		DrawPoint ();
	}

	public void DrawPoint ()
	{
		if (m_Points.Length <= 1)
			return;
		var segment = m_Resolution / (m_Points.Length - 1);
		for (int i = 0, j = 1; i < m_Points.Length; i++, j = j + 1 > m_Points.Length - 1 ? j = 0 : j + 1) {
			var point1 = m_Points [i];
			var point2 = m_Points [j];
			var direction = (point2 - point1) / m_Resolution;
			var funcDraw = MFunctionDrawDelegates [(int)m_DrawOption];
			var funcColor = MFunctionColorDelegates [(int)m_ColorOption];
			for (int x = m_Resolution * i, y = 0; x < m_Resolution * j; x++, y++) {
				_mParticlePoints [x].position = funcDraw (point1, direction, y);
				_mParticlePoints [x].startColor = m_ColorLine.Evaluate (funcColor (x, _mParticlePoints.Length));
				_mParticlePoints [x].startSize = m_Size;
			}
		}
		_mParticleSystem.SetParticles (_mParticlePoints, _mParticlePoints.Length);
	}

	private static Vector3 Linear (Vector3 point, Vector3 direction, int index)
	{
		return point + (direction * index) - _mTransform.position;
	}

	private static Vector3 Sine (Vector3 point, Vector3 direction, int index)
	{
		var result = point + (direction * index) - _mTransform.position;
		result.y += 0.5f + 0.5f * Mathf.Sin (0.5f * Mathf.PI * result.y);
		return result;
	}

	private static float Static (int index, int length)
	{
		return (float)index / length;
	}

	private static float Dynamic (int index, int length)
	{
		return (((float)index / length) + Time.time) % 1f;
	}

	#endregion

	#region Getter && Setter

	public void SetPosition (int index, Vector3 position)
	{
		if (index > m_Points.Length - 1 || index < 0 || m_Points == null)
			return;
		m_Points [index] = position;
		DrawPoint ();
	}

	public Vector3 GetPosition (int index)
	{
		if (index > m_Points.Length - 1 || index < 0 || m_Points == null)
			return Vector3.zero;
		return m_Points [index];
	}

	public void SetActive (bool value)
	{
		if (value == false) {
			_mParticleSystem.Clear ();
		} else {
			DrawPoint ();
		}
		gameObject.SetActive (value);
	}

	#endregion

}