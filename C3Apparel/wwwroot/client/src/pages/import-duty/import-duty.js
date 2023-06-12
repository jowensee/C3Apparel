import "../../scripts/global"

var Page = new Object()

function SetImportDuty(){
    const { createApp } = Vue
    let thisObject = this;
    this.el = document.getElementById("editMain");

    const endpointGet = thisObject.el.getAttribute("data-endpoint-get")
    const endpointSave = thisObject.el.getAttribute("data-endpoint-save")
    const method = thisObject.el.getAttribute("data-method")

    createApp({
        data() {
            return {
                importDutyAU:0,
                importDutyNZ:0,
                formErrorMessage:''
                    
            }
        },
        mounted(){

                this.populateForm();
            
        },
        methods:{
            
            populateForm(){

                let self = this;
                fetch(endpointGet, {
                    method: method,
                    headers:{
                        'Content-Type': 'application/json'  
                    }
                }).then(res=>res.json())
                    .then(function (response) {

                        self.importDutyAU = response.importDutyAU
                        self.importDutyNZ = response.importDutyNZ
                    });
            },
            saveImportDuty(){
                
                this.formErrorMessage = ''
                let self = this;
                let data = {
                    importDutyAU:self.importDutyAU,
                    importDutyNZ:self.importDutyNZ
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
                            self.formErrorMessage = response.message
                        }else{
                            self.formErrorMessage = response.message
                        }
                    });
            }
          
        }
    }).mount('#editMain')

}


Page.SetImportDuty = new SetImportDuty();


