using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class main : MonoBehaviour {

	public GameObject canvas;
	public GameObject sheep;
	public Slider _slider;
	public Slider sleep_meter;
	public AudioClip sheep_cry_audio;
	public AudioClip complete_audio;
	public AudioClip fail_audio;
	public AudioClip level_up_audio;
	public AudioSource audioSource;
	public int score = 0;
	public Text score_text;
	public Text time_text;
	public Text clear_text;
	public Animation anim;
	private bool is_exit = false;
	private bool is_jump = false;
	private bool is_create = false;
	private float dx = 0.0f;
	private float back_dx = 0.0f;
	private float func_a = 0.12f;
	private float c = 12.0f;
	private float delta_x;
	private int height;
	private float radian;
	public GameObject[] sheep_list;
	public ArrayList sheeps = new ArrayList();
	public ArrayList leave_sheep = new ArrayList();
	public GameObject border;

	public Vector3 init_position;
	public Quaternion init_rotatioin;
	public Vector3 init_border_position;
	public float hp_dx;
	public float sleep_meter_value;
	private float timer;
	private int current_sheep;
    float waitTime = 1.0f;

	// Use this for initialization
	void Start () {
		_slider = GameObject.Find("Slider").GetComponent<Slider>();
		sleep_meter = GameObject.Find("sleepMeter").GetComponent<Slider>();
		score_text = GameObject.Find("Text").GetComponent<Text>();
		time_text = GameObject.Find("time_text").GetComponent<Text>();
		audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.clip = sheep_cry_audio;
		init_position = sheep.transform.localPosition;
		init_rotatioin = sheep.transform.rotation;
		init_border_position = border.transform.localPosition;
		sheep = create_prefab(sheep_list[0]);
		sheeps.Add(sheep);
		current_sheep = 0;

		hp_dx = 0.01f;
	}

	float _hp = 0;
	void Update () {
		timer_func();
        if(int.Parse(score_text.text) < score) {
        	score_text.text = (int.Parse(score_text.text) + 2).ToString();
        }

		if(Input.GetMouseButtonDown(0)) {
			float value = _slider.value * 100;
			
			Debug.Log(value);
			if(value > 80) {
				jump(250, 3.6f, 0.15f, 2.0f);
				is_create = true;

				audioSource.clip = complete_audio;
				audioSource.Play();
		    	update_score();
			} else if(value > 60) {
				jump(150, 3.6f, 0.12f, 2.0f);
				if(sheeps.Count != 1) {	
					leave_sheep.Add((GameObject)sheeps[sheeps.Count - 1]);
					sheeps.RemoveAt(sheeps.Count - 1);
				}
		    	update_score();
				audioSource.clip = sheep_cry_audio;
				audioSource.Play();
			} else if(value < 60) {
				jump(50, 1.8f, 0.07f, 4.0f);
				audioSource.clip = fail_audio;
				audioSource.Play();
				if(sheeps.Count != 1) {
					leave_sheep.Add((GameObject)sheeps[sheeps.Count - 1]);
					sheeps.RemoveAt(sheeps.Count - 1);
				}

			}
        }
        if(leave_sheep.Count != 0) {
        	backflip_jump();
        }
        if(is_exit) {
    	 	is_exit = false;
    	 	hp_dx += 0.002f;

    	 	if(is_create) {
				sheeps.Add(create_prefab(sheep_list[current_sheep]));
    	 		is_create = false;
    	 	}
    	 	set_init_position();

    	 	border.transform.localPosition = init_border_position;        	 	
    	 	border.transform.Translate(0, Random.Range(-30, 30), 0);
        }
        if(is_jump) {
        	front_jump();
        }
	}

	void jump(int h, float r, float coefficient, float d_x) {
		is_jump = true;
		height = h;
		radian = r;
		func_a = coefficient;
		c = func_a * 100;
		delta_x = d_x;

	}

	void front_jump() {
		float y_1 = - Mathf.Pow((func_a * dx - c), 2) + height;
    	dx += delta_x;
		float y_2 = - Mathf.Pow((func_a * dx - c), 2) + height;

		foreach(GameObject obj in sheeps) {
        	obj.transform.position = new Vector3(obj.transform.position.x + delta_x, obj.transform.position.y + (y_2 - y_1), obj.transform.position.z);
			obj.transform.Rotate(new Vector3(0, 0, 1), radian);
		}

    	if(dx > 200.0f) {
    		is_jump = false;
    	 	set_init_position();
    		is_exit = true;
    		dx = 0.0f;
    	}
	}
	void backflip_jump() {
		float y_1 = -((func_a * back_dx + 12.0f) * (func_a * back_dx + 12.0f)) - 150;
    	back_dx -= 2.0f;
		float y_2 = -((func_a * back_dx + 12.0f) * (func_a * back_dx + 12.0f)) - 150;

		foreach(GameObject obj in leave_sheep) {
        	obj.transform.position = new Vector3(obj.transform.position.x - 2.0f, obj.transform.position.y + (y_2 - y_1), obj.transform.position.z);
			obj.transform.Rotate(new Vector3(0, 0, 1), -3.6f);
		}

    	if(back_dx < -200.0f) {
    		is_jump = false;
			foreach(GameObject obj in leave_sheep) {
				Destroy(obj);
			}
    	 	leave_sheep = new ArrayList();
    		is_exit = true;
    		back_dx = 0.0f;
    	}
	}

	void update_score() {
		foreach(GameObject obj in sheeps) {				
			score += 1 * 100;
		}		
	}

	void timer_func() {
        timer += Time.deltaTime;
        if (timer > waitTime) {
            if(int.Parse(time_text.text) <= 0) {
            } else {
                time_text.text = (int.Parse(time_text.text) - 1).ToString();
                timer = 0.0f;
            }
        }
      	 _hp += hp_dx;
    	if(_hp > 1) {
    	  _hp = 0;
    	}
    	_slider.value = _hp;
	}

	void set_init_position() {
	 	int index = 0;
		foreach(GameObject obj in sheeps) {
    	 	obj.transform.localPosition = new Vector3(init_position.x + (index * 20.0f), init_position.y - (index * 20.0f), init_position.z);
    	 	obj.transform.rotation = init_rotatioin;
    	 	index += 1;
    	}
	}

	GameObject create_prefab(GameObject sheep) {
		Vector3 init = init_position;
		GameObject obj = (GameObject)Instantiate(sheep, init_position, init_rotatioin);
		obj.transform.parent = canvas.transform;
		obj.transform.localPosition =  new Vector3(init.x + (sheeps.Count * 20.0f), init.y - (sheeps.Count * 20.0f), init.z);

		return obj;
	}
}
