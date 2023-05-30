using System;
using System.Data;
using System.Runtime.Serialization;
using System.Collections.Generic;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;
using C3Apparel.Data.Kentico.Modules.Classes;

[assembly: RegisterObjectType(typeof(ProductInfo), ProductInfo.OBJECT_TYPE)]

namespace C3Apparel.Data.Kentico.Modules.Classes
{
    /// <summary>
    /// Data container class for <see cref="ProductInfo"/>.
    /// </summary>
    [Serializable]
    public partial class ProductInfo : AbstractInfo<ProductInfo, IProductInfoProvider>
    {
        /// <summary>
        /// Object type.
        /// </summary>
        public const string OBJECT_TYPE = "c3.product";


        /// <summary>
        /// Type information.
        /// </summary>
#warning "You will need to configure the type info."
        public static readonly ObjectTypeInfo TYPEINFO = new ObjectTypeInfo(typeof(ProductInfoProvider), OBJECT_TYPE, "C3.Product", "ProductID", "ProductLastModified", "ProductGuid", "ProductGuid", "ProductName", null, null, null, null)
        {
            ModuleName = "C3Apparel",
            TouchCacheDependencies = true,
            DependsOn = new List<ObjectDependency>()
            {
                new ObjectDependency("ProductBrandID", "ecommerce.brand", ObjectDependencyEnum.NotRequired),
            },
        };


        /// <summary>
        /// Product ID.
        /// </summary>
        [DatabaseField]
        public virtual int ProductID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ProductID"), 0);
            }
            set
            {
                SetValue("ProductID", value);
            }
        }


        /// <summary>
        /// Product brand ID.
        /// </summary>
        [DatabaseField]
        public virtual int ProductBrandID
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ProductBrandID"), 0);
            }
            set
            {
                SetValue("ProductBrandID", value);
            }
        }


        /// <summary>
        /// Product name.
        /// </summary>
        [DatabaseField]
        public virtual string ProductName
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ProductName"), String.Empty);
            }
            set
            {
                SetValue("ProductName", value);
            }
        }


        /// <summary>
        /// Product code.
        /// </summary>
        [DatabaseField]
        public virtual string ProductCode
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ProductCode"), String.Empty);
            }
            set
            {
                SetValue("ProductCode", value);
            }
        }


        /// <summary>
        /// Product colour.
        /// </summary>
        [DatabaseField]
        public virtual string ProductColour
        {
            get
            {
                return ValidationHelper.GetString(GetValue("ProductColour"), String.Empty);
            }
            set
            {
                SetValue("ProductColour", value, String.Empty);
            }
        }


        /// <summary>
        /// Product price 1.
        /// </summary>
        [DatabaseField]
        public virtual decimal ProductPrice1
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("ProductPrice1"), 0m);
            }
            set
            {
                SetValue("ProductPrice1", value);
            }
        }


        /// <summary>
        /// Product MOQ units 1.
        /// </summary>
        [DatabaseField]
        public virtual int ProductMOQUnits1
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ProductMOQUnits1"), 0);
            }
            set
            {
                SetValue("ProductMOQUnits1", value);
            }
        }


        /// <summary>
        /// Product price 2.
        /// </summary>
        [DatabaseField]
        public virtual decimal ProductPrice2
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("ProductPrice2"), 0m);
            }
            set
            {
                SetValue("ProductPrice2", value);
            }
        }


        /// <summary>
        /// Product MOQ units 2.
        /// </summary>
        [DatabaseField]
        public virtual int ProductMOQUnits2
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ProductMOQUnits2"), 0);
            }
            set
            {
                SetValue("ProductMOQUnits2", value);
            }
        }


        /// <summary>
        /// Product price 3.
        /// </summary>
        [DatabaseField]
        public virtual decimal ProductPrice3
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("ProductPrice3"), 0m);
            }
            set
            {
                SetValue("ProductPrice3", value);
            }
        }


        /// <summary>
        /// Product MOQ units 3.
        /// </summary>
        [DatabaseField]
        public virtual int ProductMOQUnits3
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ProductMOQUnits3"), 0);
            }
            set
            {
                SetValue("ProductMOQUnits3", value);
            }
        }


        /// <summary>
        /// Product price 4.
        /// </summary>
        [DatabaseField]
        public virtual decimal ProductPrice4
        {
            get
            {
                return ValidationHelper.GetDecimal(GetValue("ProductPrice4"), 0m);
            }
            set
            {
                SetValue("ProductPrice4", value, 0m);
            }
        }


        /// <summary>
        /// Product MOQ units 4.
        /// </summary>
        [DatabaseField]
        public virtual int ProductMOQUnits4
        {
            get
            {
                return ValidationHelper.GetInteger(GetValue("ProductMOQUnits4"), 0);
            }
            set
            {
                SetValue("ProductMOQUnits4", value, 0);
            }
        }


        /// <summary>
        /// Product guid.
        /// </summary>
        [DatabaseField]
        public virtual Guid ProductGuid
        {
            get
            {
                return ValidationHelper.GetGuid(GetValue("ProductGuid"), Guid.Empty);
            }
            set
            {
                SetValue("ProductGuid", value);
            }
        }


        /// <summary>
        /// Product last modified.
        /// </summary>
        [DatabaseField]
        public virtual DateTime ProductLastModified
        {
            get
            {
                return ValidationHelper.GetDateTime(GetValue("ProductLastModified"), DateTimeHelper.ZERO_TIME);
            }
            set
            {
                SetValue("ProductLastModified", value);
            }
        }


        /// <summary>
        /// Deletes the object using appropriate provider.
        /// </summary>
        protected override void DeleteObject()
        {
            Provider.Delete(this);
        }


        /// <summary>
        /// Updates the object using appropriate provider.
        /// </summary>
        protected override void SetObject()
        {
            Provider.Set(this);
        }


        /// <summary>
        /// Constructor for de-serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected ProductInfo(SerializationInfo info, StreamingContext context)
            : base(info, context, TYPEINFO)
        {
        }


        /// <summary>
        /// Creates an empty instance of the <see cref="ProductInfo"/> class.
        /// </summary>
        public ProductInfo()
            : base(TYPEINFO)
        {
        }


        /// <summary>
        /// Creates a new instances of the <see cref="ProductInfo"/> class from the given <see cref="DataRow"/>.
        /// </summary>
        /// <param name="dr">DataRow with the object data.</param>
        public ProductInfo(DataRow dr)
            : base(TYPEINFO, dr)
        {
        }
    }
}