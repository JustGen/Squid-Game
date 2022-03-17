using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour, IStoreListener //для получения сообщений из Unity Purchasing
{
    [SerializeField] private GameObject _itemShopNoAds;

    IStoreController m_StoreController;

    private string noads = "com.squidgame.noads";
    private string money5999 = "com.squidgame.money5999";
    private string money10999 = "com.squidgame.money10999";
    private string money15999 = "com.squidgame.money15999";
    private string money25999 = "com.squidgame.money25999";

    void Start()
    {
        InitializePurchasing();
        RestoreVariable();

        //if (PlayerPrefs.HasKey("firstStart") == false)
        //{
        //    PlayerPrefs.SetInt("firstStart", 1);
        //    RestoreMyProduct();
        //}
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct(noads, ProductType.NonConsumable);
        builder.AddProduct(money5999, ProductType.Consumable);
        builder.AddProduct(money10999, ProductType.Consumable);
        builder.AddProduct(money15999, ProductType.Consumable);
        builder.AddProduct(money25999, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    void RestoreVariable()
    {
        if (PlayerPrefs.HasKey("ads"))
        {
            _itemShopNoAds.SetActive(false);
        }
    }

    public void BuyProduct(string productName)
    {
        m_StoreController.InitiatePurchase(productName);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        var product = args.purchasedProduct;

        if (product.definition.id == noads)
        {
            Product_NoAds();
        }

        if (product.definition.id == money5999)
        {
            GetMoney(5999);
        }

        if (product.definition.id == money10999)
        {
            GetMoney(10999);
        }

        if (product.definition.id == money15999)
        {
            GetMoney(15999);
        }

        if (product.definition.id == money25999)
        {
            GetMoney(25999);
        }

        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        return PurchaseProcessingResult.Complete;
    }

    private void Product_NoAds()
    {
        PlayerPrefs.SetInt("ads", 1);
        _itemShopNoAds.SetActive(false);
    }

    private void GetMoney(int count)
    {
        CoreGame.S.Money += count;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"In-App Purchasing initialize failed: {error}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("In-App Purchasing successfully initialized");
        m_StoreController = controller;
    }


    //public void RestoreMyProduct()
    //{
    //    if (CodelessIAPStoreListener.Instance.StoreController.products.WithID(noads).hasReceipt)
    //    {
    //        Product_NoAds();
    //    }

    //    if (CodelessIAPStoreListener.Instance.StoreController.products.WithID(vip).hasReceipt)
    //    {
    //        Product_VIP();
    //    }
    //}
}
