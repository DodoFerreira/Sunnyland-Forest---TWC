using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public  static Fade     _instantFade;

    public  Image           _imageFade;
    public  Color           _initialColor;
    public  Color           _finalColor;
    public  float           _fadeDuration;

    public  bool            _isFade;
    private float           _time;


    void Awake()
    {
	    _instantFade = this;
    }   

    
    void Start()
    {
        StartCoroutine(StartFade());
        
    }


    IEnumerator StartFade()
    {
	    _isFade = true;
	    _time = 0f;

	    while(_time <= _fadeDuration)
	        {
		        _imageFade.color = Color.Lerp(_initialColor, _finalColor, _time / _fadeDuration);
		        _time = _time + Time.deltaTime;
		        yield return null;
	        }
	
	        _isFade = false;

            gameObject.SetActive(false);
            
    }
}
