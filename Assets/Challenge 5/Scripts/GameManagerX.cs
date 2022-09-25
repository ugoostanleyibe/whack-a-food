using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour
{
	public TextMeshProUGUI timeText;
	public TextMeshProUGUI scoreText;
	public TextMeshProUGUI gameOverText;
	public GameObject titleScreen;
	public Button restartButton;

	public List<GameObject> targetPrefabs;

	private int score;
	private float secondsLeft = 60;
	private float spawnRate = 1.5f;
	public bool isGameActive;

	private readonly float spaceBetweenSquares = 2.5f;
	private readonly float minValueX = -3.75f; //  x value of the center of the left-most square
	private readonly float minValueY = -3.75f; //  y value of the center of the bottom-most square

	void Update()
	{
		if (isGameActive && secondsLeft > 0)
		{
			timeText.text = $"Time: {Mathf.Round(secondsLeft -= Time.deltaTime)}";
		}
		else if (isGameActive)
		{
			GameOver();
		}
	}

	// Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
	public void StartGame(int difficulty)
	{
		spawnRate /= difficulty;
		isGameActive = true;
		StartCoroutine(SpawnTarget());
		score = 0;
		UpdateScore(0);
		titleScreen.SetActive(false);
	}

	// While game is active spawn a random target
	IEnumerator SpawnTarget()
	{
		while (isGameActive)
		{
			yield return new WaitForSeconds(spawnRate);
			int index = Random.Range(0, targetPrefabs.Count);

			if (isGameActive)
			{
				Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
			}
		}
	}

	// Generate a random spawn position based on a random index from 0 to 3
	Vector3 RandomSpawnPosition()
	{
		float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
		float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);
		return new(spawnPosX, spawnPosY, 0.0f);
	}

	// Generates random square index from 0 to 3, which determines which square the target will appear in
	int RandomSquareIndex() => Random.Range(0, 4);

	// Update score with value from target clicked
	public void UpdateScore(int scoreToAdd) => scoreText.text = $"Score: {score += scoreToAdd}";

	// Stop game, bring up game over text and restart button
	public void GameOver()
	{
		restartButton.gameObject.SetActive(true);
		gameOverText.gameObject.SetActive(true);
		isGameActive = false;
	}

	// Restart game by reloading the scene
	public void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
