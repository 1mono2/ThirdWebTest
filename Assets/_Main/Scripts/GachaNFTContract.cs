
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Thirdweb;
using UnityEngine;


public class GachaNFTContract : MonoBehaviour
{

    private ThirdwebSDK _sdk;
    private const string _contractAdress = "0x5c156dBdAcFe756D1c28f9DA4d9927f5fbdaA92e";
    private string _abi = "[{ \"inputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"constructor\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"nftContract\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"price\", \"type\": \"uint256\" } ], \"name\": \"deposit\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"offerId\", \"type\": \"uint256\" } ], \"name\": \"getOffer\", \"outputs\": [ { \"components\": [ { \"internalType\": \"address\", \"name\": \"nftContract\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"price\", \"type\": \"uint256\" } ], \"internalType\": \"struct GachaNFT.Offer\", \"name\": \"\", \"type\": \"tuple\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"getOffersByOwner\", \"outputs\": [ { \"components\": [ { \"internalType\": \"address\", \"name\": \"nftContract\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"price\", \"type\": \"uint256\" } ], \"internalType\": \"struct GachaNFT.Offer[]\", \"name\": \"\", \"type\": \"tuple[]\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"name\": \"offers\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"nftContract\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"price\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" }, { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" }, { \"internalType\": \"bytes\", \"name\": \"\", \"type\": \"bytes\" } ], \"name\": \"onERC721Received\", \"outputs\": [ { \"internalType\": \"bytes4\", \"name\": \"\", \"type\": \"bytes4\" } ], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"offerId\", \"type\": \"uint256\" }, { \"internalType\": \"uint256\", \"name\": \"maxPrice\", \"type\": \"uint256\" } ], \"name\": \"purchase\", \"outputs\": [], \"stateMutability\": \"payable\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"maxPrice\", \"type\": \"uint256\" } ], \"name\": \"selectRandomNFT\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"nftContractAddress\", \"type\": \"address\" }, { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"withdraw\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }]";

    public async void Deposit(string address, uint tokenId, uint priceEth)
    {
        var contract = ThirdwebManager.Instance.SDK.GetContract(_contractAdress, _abi);
        
        var priceWei = priceEth.ToString().ToWei();
        var transactionResult  = await contract.Write("deposit", address, (BigInteger)tokenId, priceWei);
        if (transactionResult.isSuccessful())
        {
            Debug.Log("deposit success!");
        }
    }

    public async void GetOffersByOwner()
    {
        var contract = ThirdwebManager.Instance.SDK.GetContract(_contractAdress, _abi);
        
        var offers  = await contract.Read<List<Offer>>("withdraw");
        Debug.Log("offers: " + offers.Count);
        
    }
    
    public async void Withdraw(string nftContractAddress, uint tokenId)
    {
        var contract = ThirdwebManager.Instance.SDK.GetContract(_contractAdress, _abi);
        
        var transactionResult  = await contract.Write("withdraw", nftContractAddress ,(BigInteger)tokenId);
        if (transactionResult.isSuccessful())
        {
            Debug.Log("withdraw success!");
        }
    }

    public async void CallSelectRandomNFT()
    {
        await SelectRandomNft(2);
    }

    public async Task<BigInteger> SelectRandomNft(uint maxPriceEth)
    {
        var contract = ThirdwebManager.Instance.SDK.GetContract(_contractAdress, _abi);
        
        var maxPriceWei = maxPriceEth.ToString().ToWei();
        var offerId  = await contract.Read<BigInteger>("selectRandomNFT", maxPriceWei);
        Debug.Log("offerId: " + offerId);
        return offerId;
    }

    public async void Purchase(BigInteger offerId, uint maxPriceEth)
    {
        var contract = ThirdwebManager.Instance.SDK.GetContract(_contractAdress, _abi);
        
        var maxPriceWei = maxPriceEth.ToString().ToWei();
        var transactionResult  = await contract.Write("purchase", offerId, maxPriceWei);
        if (transactionResult.isSuccessful())
        {
            Debug.Log("withdraw success!");
        }
    }
        
    public async Task<Offer> getOffer(BigInteger offerId)
    {
        var contract = ThirdwebManager.Instance.SDK.GetContract(_contractAdress, _abi);
        
         var offer = await contract.Read<Offer>("getOffer", offerId);
         return offer;
    }
        
        
    public struct Offer
    {
        public string nftContract { get; set; }
        
        public string owner { get; set; }
        
        public BigInteger tokenId { get; set; }
        
        public BigInteger price { get; set; }
    }
}
