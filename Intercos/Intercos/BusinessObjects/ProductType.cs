using YubikStudioCore;
using YubikStudioCore.Documents;
using YubikStudioCore.Attributes;

namespace Intercos.BusinessObjects
{
  [Flags]
public enum ProductType
{
  Cream = 1,
  Serum = 2,
  Shampoo = 4,
  Lotion = 8,
  Cleanser = 16,
  Mask = 32,
  Other = 64
}

}
