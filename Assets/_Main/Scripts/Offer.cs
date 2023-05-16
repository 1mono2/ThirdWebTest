using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;

[FunctionOutput]
public class Offer
{
    [Parameter("address", "nftContract", 1)]
    public string nftContract { get; set; }
    
    [Parameter("address", "owner", 2)]
    public string owner { get; set; }

    [Parameter("uint256", "tokenId", 3)]
    public BigInteger tokenId { get; set; }
    
    [Parameter("uint256", "price", 4)]
    public BigInteger price { get; set; }
}
