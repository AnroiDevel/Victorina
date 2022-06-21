using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Victorina;


public class AndroidIAPExample : MonoBehaviour, IStoreListener
{
    #region Fields

    [SerializeField] private Button[] _payBtns;
    [SerializeField] private GameObject _loadShopPanel;

    private List<CatalogItem> Catalog;

    private static IStoreController m_StoreController;

    #endregion


    #region Properties

    public bool IsInitialized => m_StoreController != null && Catalog != null;

    #endregion


    #region UnityMethods

    public void Start()
    {
        Login();
        _loadShopPanel.SetActive(true);
    }

    #endregion


    #region Methods

    public void BuyLot(int numberLot)
    {
        BuyProductID(Catalog[numberLot].ItemId);
    }

    private void Login()
    {
        _loadShopPanel.SetActive(true);

        PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
        {
            CreateAccount = true,
            AndroidDeviceId = SystemInfo.deviceUniqueIdentifier
        }, result =>
        {
            RefreshIAPItems();
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    private void RefreshIAPItems()
    {
        PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), result =>
        {
            Catalog = result.Catalog;
            InitializePurchasing();
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    public void InitializePurchasing()
    {
        if (m_StoreController != null)
        {
            _loadShopPanel.SetActive(false);
            InitPayBtns();
        }

        if (IsInitialized) return;

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(AppStore.GooglePlay));

        foreach (var item in Catalog)
        {
            builder.AddProduct(item.ItemId, ProductType.Consumable);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;

        _loadShopPanel.SetActive(false);
        InitPayBtns();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        if (!IsInitialized)
        {
            return PurchaseProcessingResult.Complete;
        }

        if (e.purchasedProduct == null)
        {
            Debug.LogWarning("Attempted to process purchase with unknown product. Ignoring");
            return PurchaseProcessingResult.Complete;
        }

        if (string.IsNullOrEmpty(e.purchasedProduct.receipt))
        {
            Debug.LogWarning("Attempted to process purchase with no receipt: ignoring");
            return PurchaseProcessingResult.Complete;
        }

        Debug.Log("Processing transaction: " + e.purchasedProduct.transactionID);

        var googleReceipt = GooglePurchase.FromJson(e.purchasedProduct.receipt);

        PlayFabClientAPI.ValidateGooglePlayPurchase(new ValidateGooglePlayPurchaseRequest()
        {
            CurrencyCode = e.purchasedProduct.metadata.isoCurrencyCode,
            PurchasePrice = (uint)(e.purchasedProduct.metadata.localizedPrice * 100),
            ReceiptJson = googleReceipt.PayloadData.json,
            Signature = googleReceipt.PayloadData.signature
        }, result => PlayFabAccountManager.Instance.GetPlayerInventory(),
           error => Debug.Log("Validation failed: " + error.GenerateErrorReport())
        );


        return PurchaseProcessingResult.Complete;
    }

    private void InitPayBtns()
    {
        Product product = m_StoreController.products.WithID("bandle500bit");
        if (product != null)
        {
            var price = product.metadata.localizedPriceString;
            price = price.Remove(price.Length - 1);
            _payBtns[0].GetComponentInChildren<TMP_Text>().text = price + "RUB";
        }

        Product product2 = m_StoreController.products.WithID("bundle50000bit");
        if (product2 != null)
        {
            var price = product2.metadata.localizedPriceString;
            price = price.Remove(price.Length - 1);

            _payBtns[1].GetComponentInChildren<TMP_Text>().text = price + "RUB";
        }

        Product product3 = m_StoreController.products.WithID("notreclama");
        if (product3 != null)
        {
            var price = product3.metadata.localizedPriceString;
            price = price.Remove(price.Length - 1);

            _payBtns[2].GetComponentInChildren<TMP_Text>().text = price + "RUB";
        }

        Product product4 = m_StoreController.products.WithID("vipday");
        if (product4 != null)
        {
            var price = product4.metadata.localizedPriceString;
            price = price.Remove(price.Length - 1);

            _payBtns[3].GetComponentInChildren<TMP_Text>().text = price + "RUB";
        }
    }

    void BuyProductID(string productId)
    {
        if (!IsInitialized) throw new Exception("IAP Service is not initialized!");

        m_StoreController.InitiatePurchase(productId);
    }

    #endregion
}
