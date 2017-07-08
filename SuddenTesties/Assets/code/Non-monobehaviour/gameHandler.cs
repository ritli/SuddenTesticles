using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class gameHandler {

	static List<Vector2> m_spawnPoints;
	static int m_p1score, m_p2score;
	static public int P1Score {
		get { return m_p1score; }
	}
	static public int P2Score {
		get { return m_p2score; }
	}

	static GUIText m_p1ScoreText, m_p2ScoreText;

	// Setup new game
	public static void instantiate(){
		m_spawnPoints = new List<Vector2> ();
		// Load spawnpoints

		// Load map

		// Load and start music

		// Instantiate players, give IDs and give spawnpoints
		
	}

	static public Vector2 playerDeath(int playerId, bool points = true){
		if (points) {
			playerId == 1 ? m_p2score++ : m_p1score++;
		}

		return m_spawnPoints [Random.Range (0, m_spawnPoints.Count)];
	}

	static public void scoreText(GUIText textObj, int playerId){
		playerId == 1 ? m_p1ScoreText = textObj : m_p2ScoreText = textObj;
	}
}
