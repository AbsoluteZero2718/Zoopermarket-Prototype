using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int minItemsToBuy = 1;
    [SerializeField] private int maxItemsToBuy = 3;
    [SerializeField] private int queueSwitchThreshold = 2;

    private enum State
    {
        Shopping,
        ApproachingLine,
        InQueue,
        CheckingOut,
        Exiting
    }

    private State currentState;
    private Cashier targetCashier;
    private Transform exitPoint;
    private Vector3 currentMoveTargetPosition;
    private bool hasTargetPosition = false;

    private List<Shelf> shoppingList = new List<Shelf>();
    private List<Product> shoppingCart = new List<Product>();
    private int shelfVisitIndex = 0;

    private Coroutine checkQueueCoroutine;

    public void Initialize(Cashier cashier, Transform exit)
    {
        targetCashier = cashier;
        exitPoint = exit;

        CreateShoppingList();

        if (shoppingList.Count > 0)
        {
            currentState = State.Shopping;
            SetNextShoppingDestination();
        }
        else
        {
            LeaveStore(false);
        }
    }

    private void Update()
    {
        if (currentState == State.ApproachingLine)
        {
            currentMoveTargetPosition = targetCashier.GetLineTailPosition();
            MoveTowardsTarget();

            if (Vector2.Distance(transform.position, currentMoveTargetPosition) < 0.1f)
            {
                currentState = State.InQueue;
                targetCashier.JoinQueue(this);
                checkQueueCoroutine = StartCoroutine(CheckForBetterQueue());
            }
            return;
        }

        if (!hasTargetPosition) return;

        MoveTowardsTarget();

        if (Vector2.Distance(transform.position, currentMoveTargetPosition) < 0.1f)
        {
            OnDestinationReached();
        }
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            currentMoveTargetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    private void OnDestinationReached()
    {
        hasTargetPosition = false;

        if (currentState == State.Shopping)
        {
            Shelf currentShelf = shoppingList[shelfVisitIndex];
            Product takenProduct = currentShelf.TakeProduct();

            if (takenProduct != null)
            {
                shoppingCart.Add(takenProduct);
            }

            shelfVisitIndex++;
            SetNextShoppingDestination();
        }
        else if (currentState == State.CheckingOut)
        {
            targetCashier.OnCustomerArrivedAtCounter(this, shoppingCart);
        }
        else if (currentState == State.Exiting)
        {
            Destroy(gameObject);
        }
    }

    private void CreateShoppingList()
    {
        Shelf[] allShelvesArray = FindObjectsByType<Shelf>(FindObjectsSortMode.None);
        List<Shelf> availableShelves = new List<Shelf>(allShelvesArray);

        if (availableShelves.Count == 0) return;

        int itemsToTryToGet = Random.Range(minItemsToBuy, maxItemsToBuy + 1);

        for (int i = 0; i < itemsToTryToGet; i++)
        {
            if (availableShelves.Count == 0) break;

            int randomIndex = Random.Range(0, availableShelves.Count);
            shoppingList.Add(availableShelves[randomIndex]);

            availableShelves.RemoveAt(randomIndex);
        }
    }

    private void SetNextShoppingDestination()
    {
        if (shelfVisitIndex < shoppingList.Count)
        {
            currentMoveTargetPosition = shoppingList[shelfVisitIndex].transform.position;
            hasTargetPosition = true;
        }
        else
        {
            if (shoppingCart.Count > 0)
            {
                currentState = State.ApproachingLine;
            }
            else
            {
                LeaveStore(false);
            }
        }
    }

    public void SetQueuePosition(Vector3 newPosition)
    {
        if (currentState == State.Exiting) return;

        currentState = State.InQueue;
        currentMoveTargetPosition = newPosition;
        hasTargetPosition = true;
    }

    public void GoToCheckout(Vector3 checkoutPos)
    {
        if (checkQueueCoroutine != null)
        {
            StopCoroutine(checkQueueCoroutine);
        }
        currentState = State.CheckingOut;
        currentMoveTargetPosition = checkoutPos;
        hasTargetPosition = true;
    }

    public void LeaveStore(bool didPurchase)
    {
        currentState = State.Exiting;
        if (exitPoint != null)
        {
            currentMoveTargetPosition = exitPoint.position;
            hasTargetPosition = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator CheckForBetterQueue()
    {
        while (currentState == State.InQueue)
        {
            yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));

            Cashier bestCashier = CashierManager.Instance.FindBestCashier();
            if (bestCashier != null && bestCashier != targetCashier)
            {
                if (targetCashier.QueueCount > bestCashier.QueueCount + queueSwitchThreshold)
                {
                    targetCashier.LeaveQueue(this);
                    targetCashier = bestCashier;
                    currentState = State.ApproachingLine;
                    yield break;
                }
            }
        }
    }
}