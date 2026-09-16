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
            Debug.LogError($"[IAP] Init failed: {e}");
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
            Debug.LogError($"[IAP] Purchase error: {e}");
            OnPurchaseFailed?.Invoke("Не удалось начать покупку");
        }
    }

    private void HandleProductsFetched(List<Product> products)
    {
        _initialized = true;
        Debug.Log("[IAP] Ready to purchase");
    }

    private void HandleProductsFetchFailed(ProductFetchFailed failure)
    {
        Debug.LogError($"[IAP] Fetch failed: {failure.FailureReason}");
        OnPurchaseFailed?.Invoke("Товар недоступен в магазине");
    }

    private void HandlePurchasePending(PendingOrder order)
    {
        Debug.Log("[IAP] Purchase succeeded");
        _store.ConfirmPurchase(order);
        OnPurchaseSucceeded?.Invoke();
    }

    private void HandlePurchaseFailed(FailedOrder order)
    {
        Debug.LogWarning($"[IAP] Purchase failed: {order.FailureReason}");
        OnPurchaseFailed?.Invoke(GetFriendlyMessage(order.FailureReason));
    }

    private void HandleStoreDisconnected(StoreConnectionFailureDescription failure)
    {
        Debug.LogError($"[IAP] Store disconnected: {failure.Message}");
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