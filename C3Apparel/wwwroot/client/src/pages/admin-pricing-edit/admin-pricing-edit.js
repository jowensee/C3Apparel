import "../../scripts/global"

var Page = new Object()

function PricingEdit(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("editMain");

    const endpointGet = thisObject.el.getAttribute("data-endpoint-get")
    const endpointSave = thisObject.el.getAttribute("data-endpoint-save")
    const method = thisObject.el.getAttribute("data-method")

    createApp({

        data() {
            return {
                pricing:{
                    id:Number(thisObject.el.getAttribute("data-id")),
                    status:'',
                    brandId:0,
                    collection:'',
                    c3Style:'',
                    supplierStyle:'',
                    description:'',
                    coo:'',
                    productGroup:'',
                    sizes:'',
                    productColours:'',
                    colourDescriptions:'',
                    buyPrice:0,
                    skuWeight:0,
                    c3OverrideWeight:0,
                },
                formErrorMessage:'',
                showSuccess:false,
                errors:[]
                    
            }
        },
        mounted(){
            if (this.pricing.id > 0){

                this.populateForm(this.pricing.id);
            }
            
        },
        methods:{
            
            populateForm(id){
                let data = {
                    id:id,

                }
                let pricing = this.pricing;
                fetch(endpointGet, {
                    method: method,
                    headers:{
                        'Content-Type': 'application/json'  
                    },
                    body: method == "GET" ? null : JSON.stringify(data),
                    data: method != "GET" ? null : JSON.stringify(data),
                }).then(res=>res.json())
                    .then(function (response) {

                        pricing.status = response.productPricing.status
                        pricing.brandId = response.productPricing.brandId
                        pricing.collection = response.productPricing.collection
                        pricing.c3Style = response.productPricing.c3Style
                        pricing.supplierStyle = response.productPricing.supplierStyle
                        pricing.description = response.productPricing.description
                        pricing.coo = response.productPricing.coo
                        pricing.productGroup = response.productPricing.productGroup
                        pricing.sizes = response.productPricing.sizes
                        pricing.productColours = response.productPricing.colour
                        pricing.colourDescriptions = response.productPricing.colourDescription
                        pricing.buyPrice = response.productPricing.c3BuyPrice
                        pricing.skuWeight = response.productPricing.skuWeight
                        pricing.c3OverrideWeight = response.productPricing.c3OverrideWeight
                    });
            },
            validiteForm(){

                this.errors = []
                let validate = true

                if (this.pricing.brandId == 0){
                    this.errors.push("<b>Brand:</b> Please select one")
                    validate = false
                }

                if (this.pricing.c3Style.trim() == ''){
                    this.errors.push("<b>C-3 Style:</b> Please enter a value")
                    validate = false
                }

                if (this.pricing.description.trim() == ''){
                    this.errors.push("<b>Description:</b> Please enter a value")
                    validate = false
                }

                if (this.pricing.productGroup.trim() == ''){
                    this.errors.push("<b>Product Group:</b> Please enter a value")
                    validate = false
                }

                if (this.pricing.sizes.trim() == ''){
                    this.errors.push("<b>Sizes:</b> Please enter a value")
                    validate = false
                }

                if (this.pricing.colourDescriptions.trim() == ''){
                    this.errors.push("<b>Colour Descriptions:</b> Please enter a value")
                    validate = false
                }
                if (isNaN(this.pricing.buyPrice)){
                    this.errors.push("<b>Buy Price:</b> Please enter a decimal value")
                    validate = false
                }

                if (isNaN(this.pricing.skuWeight)){
                    this.errors.push("<b>SKU Weight:</b> Please enter a decimal value")
                    validate = false
                }

                if (isNaN(this.pricing.c3OverrideWeight)){
                    this.errors.push("<b>C3 Override Weight:</b> Please enter a decimal value")
                    validate = false
                }
                return validate
            },
            savePricing(){

                this.showSuccess = false
                if (!this.validiteForm()){
                    return
                }
                
                let self = this;
                let data = {
                    id:self.pricing.id, 
                    status: self.pricing.status,
                    brandId:self.pricing.brandId,
                    collection:self.pricing.collection,
                    c3Style:self.pricing.c3Style,
                    supplierStyle:self.pricing.supplierStyle,
                    description:self.pricing.description,
                    coo:self.pricing.coo,
                    productGroup:self.pricing.productGroup,
                    sizes:self.pricing.sizes,
                    colour:self.pricing.productColours,
                    colourDescription:self.pricing.colourDescriptions,
                    c3BuyPrice:self.pricing.buyPrice,
                    skuWeight:self.pricing.skuWeight,
                    c3OverrideWeight:self.pricing.c3OverrideWeight
                }
                
                fetch(endpointSave, {
                    method: method,
                    headers:{
                        'Content-Type': 'application/json'  
                    },
                    body: method == "GET" ? null : JSON.stringify(data),
                    data: method != "GET" ? null : JSON.stringify(data),
                }).then(res=>res.json())
                    .then(function (response) {

                        if (response.success){
                            self.showSuccess = true
                        }else{
                            self.formErrorMessage = response.message
                        }
                    });
            }
          
        }
    }).mount('#editMain')

}


Page.PricingEdit = new PricingEdit();


