using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IapService : MonoBehaviour
{
    public event Action OnPurchaseSucceeded;
    public event Action<string> OnPurchaseFailed;

    private const string ProductId = "coins_pack_small";

    private StoreController _store;
    private bool _initialized;

    private void Awake()
    {
        try
        {
            _store = UnityIAPServices.StoreController();

            _store.OnPurchasePending += HandlePurchasePending;
            _store.OnPurchaseFailed += HandlePurchaseFailed;
            _store.OnStoreDisconnected += HandleStoreDisconnected;
            _store.OnProductsFetched += HandleProductsFetched;
            _store.OnProductsFetchFailed += HandleProductsFetchFailed;

            _store.Connect();          // ← без await

            _store.FetchProducts(new List<ProductDefinition>
        {
            new ProductDefinition(ProductId, ProductType.Consumable)
        });
        }
        catch (Exception e)
        {
            OnPurchaseFailed?.Invoke("Не удалось подключиться к магазину");
        }
    }

    public void BuyCoins()
    {
        if (!_initialized || _store == null)
        {
            OnPurchaseFailed?.Invoke("Магазин ещё не готов");
            return;
        }

        try
        {
            _store.PurchaseProduct(ProductId);
        }
        catch (Exception e)
        {
            OnPurchaseFailed?.Invoke("Не удалось начать покупку");
        }
    }

    private void HandleProductsFetched(List<Product> products)
    {
        _initialized = true;
    }

    private void HandleProductsFetchFailed(ProductFetchFailed failure)
    {
        OnPurchaseFailed?.Invoke("Товар недоступен в магазине");
    }

    private void HandlePurchasePending(PendingOrder order)
    {
        _store.ConfirmPurchase(order);

        Analytics.Track("purchase_succeeded", new Dictionary<string, object>
    {
        { "product", ProductId }
    });

        OnPurchaseSucceeded?.Invoke();
    }

    private void HandlePurchaseFailed(FailedOrder order)
    {
        Debug.LogWarning($"[IAP] Purchase failed: {order.FailureReason}");

        Analytics.Track("purchase_failed", new Dictionary<string, object>
    {
        { "reason", order.FailureReason.ToString() }
    });

        OnPurchaseFailed?.Invoke(GetFriendlyMessage(order.FailureReason));
    }

    private void HandleStoreDisconnected(StoreConnectionFailureDescription failure)
    {
        _initialized = false;
        OnPurchaseFailed?.Invoke("Магазин недоступен");
    }

    private string GetFriendlyMessage(PurchaseFailureReason reason)
    {
        return reason switch
        {
            PurchaseFailureReason.UserCancelled => "Покупка отменена",
            PurchaseFailureReason.PaymentDeclined => "Платёж отклонён",
            PurchaseFailureReason.ProductUnavailable => "Товар недоступен",
            PurchaseFailureReason.DuplicateTransaction => "Уже куплено",
            PurchaseFailureReason.ExistingPurchasePending => "Покупка уже в процессе",
            PurchaseFailureReason.SignatureInvalid => "Ошибка подписи",
            _ => "Не удалось совершить покупку"
        };
    }

    private void OnDestroy()
    {
        if (_store == null) return;

        _store.OnPurchasePending -= HandlePurchasePending;
        _store.OnPurchaseFailed -= HandlePurchaseFailed;
        _store.OnStoreDisconnected -= HandleStoreDisconnected;
        _store.OnProductsFetched -= HandleProductsFetched;
        _store.OnProductsFetchFailed -= HandleProductsFetchFailed;
    }
}