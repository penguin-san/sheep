using UnityEngine;
using System.Collections;

public class Data  
{
	public readonly static Data Instance = new Data();

	public int best_score = 0;
	public int score = 0;
	public string name = string.Empty;
}