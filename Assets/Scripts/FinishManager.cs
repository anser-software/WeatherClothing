using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;

public class FinishManager : MonoBehaviour
{

    [SerializeField]
    private GameObject positiveReactionPrefab, negativeReactionPrefab, scoreBoardPrefab, confettiFX;

    [SerializeField]
    private float transitionDuration, delayBetweenReactions, reactionScale, reactionScaleDuration, rotateCamSpeed, showoffDuration;

    [SerializeField]
    private Vector3 cameraOffsetFromTarget, targetOffset, reactionOffsetFromClothing, scoreBoradOffsetFromCamera, scoreBoardInterval;

    private bool rotateCam = false;

    private Vector3 cameraRotationPivot;

    private Transform player, enemy, mainCamera;

    private void Start()
    {
        GameplayManager.instance.OnGameStateChanged += () =>
        {
            if (GameplayManager.instance.GameState == GameState.Finish)
                InitiateFinish();
        };

        player = FindObjectOfType<PlayerController>().transform;

        enemy = FindObjectOfType<EnemyController>().transform;

        mainCamera = Camera.main.transform;
    }

    private void InitiateFinish()
    {
        EnemyShowoff();
    }

    private void EnemyShowoff()
    {
        mainCamera.DOMove(enemy.position + targetOffset + cameraOffsetFromTarget, transitionDuration).SetEase(Ease.InOutSine);

        var targetRotation = Quaternion.LookRotation(-cameraOffsetFromTarget.normalized);

        mainCamera.DORotateQuaternion(targetRotation, transitionDuration).SetEase(Ease.InOutSine).OnComplete(() => StartCoroutine(EnemyReactions()));
    }

    private void PlayerShowoff()
    {
        mainCamera.DOMove(player.position + targetOffset + cameraOffsetFromTarget, transitionDuration).SetEase(Ease.InOutSine);

        var targetRotation = Quaternion.LookRotation(-cameraOffsetFromTarget.normalized);

        mainCamera.DORotateQuaternion(targetRotation, transitionDuration).SetEase(Ease.InOutSine).OnComplete(() => StartCoroutine(PlayerReactions()));
    }

    private void Update()
    {
        if(rotateCam)
        {
            RotateAround(mainCamera, cameraRotationPivot, Vector3.up, Time.deltaTime * rotateCamSpeed);
        }
    }

    static void RotateAround(Transform transform, Vector3 pivotPoint, Vector3 axis, float angle)
    {
        Quaternion rot = Quaternion.AngleAxis(angle, axis);
        transform.position = rot * (transform.position - pivotPoint) + pivotPoint;
        transform.rotation = rot * transform.rotation;
    }

    private IEnumerator PlayerReactions()
    {
        cameraRotationPivot = player.position + targetOffset;

        rotateCam = true;

        var activatedClothing = GameplayManager.instance.playerClothingManager.allClothingPieces.Where(c => c.activated);

        foreach (var clothing in activatedClothing)
        {
            GameObject reaction;

            if (clothing.correctWeather)
            {
                reaction = Instantiate(positiveReactionPrefab);
            }
            else
            {
                reaction = Instantiate(negativeReactionPrefab);
            }


            var reactionEmojiPosition = clothing.visualsPos + reactionOffsetFromClothing;

            if (clothing.ClothingType == ClothingType.Head)
                reactionEmojiPosition += Vector3.up * 0.2F;


            var pointingLine = reaction.transform.GetComponentInChildren<LineRenderer>();

            pointingLine.transform.parent = null;

            pointingLine.positionCount = 2;

            pointingLine.SetPositions(new Vector3[]{ reactionEmojiPosition, clothing.visualsPos });


            reaction.transform.position = reactionEmojiPosition;

            reaction.transform.forward = (mainCamera.position - reaction.transform.position).normalized;

            reaction.transform.localScale = Vector3.zero;

            reaction.transform.DOScale(reactionScale, reactionScaleDuration).SetEase(Ease.InOutElastic);

            yield return new WaitForSeconds(delayBetweenReactions);
        }

        var correctClothingCount = activatedClothing.Where(c => c.correctWeather).Count();

        var accuracy = correctClothingCount / (float)activatedClothing.Count();

        for (int i = 0; i < 3; i++)
        {
            var scoreBoard = Instantiate(scoreBoardPrefab);

            var score = Mathf.Clamp(Random.Range(2, 4) + Mathf.RoundToInt(accuracy * 10F), 5, 10);

            scoreBoard.transform.GetComponentInChildren<Text>().text = score.ToString();

            scoreBoard.transform.parent = mainCamera;

            scoreBoard.transform.localPosition = scoreBoradOffsetFromCamera + scoreBoardInterval * i - Vector3.up;

            var scoreBoardTransSeq = DOTween.Sequence();

            scoreBoardTransSeq.Append(scoreBoard.transform.DOLocalMove(scoreBoradOffsetFromCamera + scoreBoardInterval * i, delayBetweenReactions));

            scoreBoardTransSeq.Join(scoreBoard.transform.DOLocalRotate(Vector3.zero, delayBetweenReactions));

            scoreBoardTransSeq.AppendInterval(showoffDuration);

            scoreBoardTransSeq.Append(scoreBoard.transform.DOBlendableLocalMoveBy(Vector3.up * -1F, delayBetweenReactions));

            scoreBoardTransSeq.Join(scoreBoard.transform.DOLocalRotate(new Vector3(0F, 180F, 0F), delayBetweenReactions));

            scoreBoardTransSeq.AppendCallback(() => Destroy(scoreBoard));

            yield return new WaitForSeconds(delayBetweenReactions);
        }

        FinishCompleted();
    }

    private void FinishCompleted()
    {
        var confetti = Instantiate(confettiFX, player.position + Vector3.up * 3F, Quaternion.identity);

        GameplayManager.instance.FinishCompleted();
    }

    private IEnumerator EnemyReactions()
    {
        cameraRotationPivot = enemy.position + targetOffset;

        rotateCam = true;

        var activatedClothing = GameplayManager.instance.enemyClothingManager.allClothingPieces.Where(c => c.activated);

        foreach (var clothing in activatedClothing)
        {
            GameObject reaction;

            if(clothing.correctWeather)
            {
                reaction = Instantiate(positiveReactionPrefab);
            } else
            {
                reaction = Instantiate(negativeReactionPrefab);
            }


            var reactionEmojiPosition = clothing.visualsPos + reactionOffsetFromClothing;

            if (clothing.ClothingType == ClothingType.Head)
                reactionEmojiPosition += Vector3.up * 0.2F;


            var pointingLine = reaction.transform.GetComponentInChildren<LineRenderer>();

            pointingLine.transform.parent = null;

            pointingLine.positionCount = 2;

            pointingLine.SetPositions(new Vector3[] { reactionEmojiPosition, clothing.visualsPos });


            reaction.transform.position = reactionEmojiPosition;

            reaction.transform.forward = (mainCamera.position - reaction.transform.position).normalized;

            reaction.transform.localScale = Vector3.zero;

            reaction.transform.DOScale(reactionScale, reactionScaleDuration).SetEase(Ease.InOutElastic);

            yield return new WaitForSeconds(delayBetweenReactions);
        }

        var scoreMid = Random.Range(4, 7);

        for (int i = 0; i < 3; i++)
        {
            var scoreBoard = Instantiate(scoreBoardPrefab);

            var score = Mathf.Clamp(Random.Range(scoreMid - 3, scoreMid + 3), 1, 9);

            scoreBoard.transform.GetComponentInChildren<Text>().text = score.ToString();

            scoreBoard.transform.parent = mainCamera;

            scoreBoard.transform.localPosition = scoreBoradOffsetFromCamera + scoreBoardInterval * i - Vector3.up;

            var scoreBoardTransSeq = DOTween.Sequence();

            scoreBoardTransSeq.Append(scoreBoard.transform.DOLocalMove(scoreBoradOffsetFromCamera + scoreBoardInterval * i, delayBetweenReactions));

            scoreBoardTransSeq.Join(scoreBoard.transform.DOLocalRotate(Vector3.zero, delayBetweenReactions));

            scoreBoardTransSeq.AppendInterval(showoffDuration);

            scoreBoardTransSeq.Append(scoreBoard.transform.DOBlendableLocalMoveBy(Vector3.up * -1F, delayBetweenReactions));

            scoreBoardTransSeq.Join(scoreBoard.transform.DOLocalRotate(new Vector3(0F, 180F, 0F), delayBetweenReactions));

            scoreBoardTransSeq.AppendCallback(() => Destroy(scoreBoard));

            yield return new WaitForSeconds(delayBetweenReactions);
        }

        yield return new WaitForSeconds(showoffDuration);

        rotateCam = false;

        PlayerShowoff();
    }

}
