var form_create = document.getElementById("create");
document.getElementById("create-btn").addEventListener('click', function (e) {    
    onSendRequestEnding(form_create);
});

document.getElementById("add-btn").addEventListener('click', function (e) {    
    onSendRequest(form_create);
});


onSendRequestEnding = function (form) {
    let url = '/Order/Save';
    let request = serializeForm(form);
    let xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.onload = function (e) {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                window.location.replace("/Order/");
            } else {
                console.log("No se pudo conectar al servidor.");
            }
        }
    };
    xhr.onerror = function (ex) {
        console.log("Error", ex.message);
    };
    xhr.send(request);
};

onSendRequest = function (form) {
    let url = '/Order/Add';
    let request = serializeForm(form);
    let xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.onload = function (e) {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                onRenderRequest(xhr.responseText);
            } else {
                console.log("No se pudo conectar al servidor.");
            }
        }
    };
    xhr.onerror = function (ex) {
        console.log("Error", ex.message);
    };
    xhr.send(request);
};
onRenderRequest = function (response) {
    let modal = document.querySelector("#createModal");
    let content = modal.querySelector(".modal-content");

    $("#createModal").modal('show');

    let body = content.querySelector("#detail-item-add");
    if (!body)
        content.insertAdjacentHTML('beforeend', response);
    else {
        body.outerHTML = response;
    }
    
    document.getElementById("Item_UnitPrice").addEventListener('input', onRecalcule);
    document.getElementById("Item_Qty").addEventListener('input', onRecalcule);
    document.getElementById("Item_ProductId").addEventListener('input', function () {
        document.getElementById('description').value = this.options[this.selectedIndex].text;
    });
    var formdetail = document.getElementById("detail-item-add");
    formdetail.addEventListener('submit', function (e) {
        document.getElementById('Order_Id').value = document.getElementById('Item_OrderId').value;
        document.getElementById('Item_Description').value = document.getElementById('description').value;
        onSendRequestDetail(formdetail);
        return false;
    });
}
onRecalcule = function () {
    let unitPrice = document.getElementById('Item_UnitPrice');
    let qty = document.getElementById('Item_Qty');
    let total = document.getElementById('Item_Total');
    total.value = unitPrice.value * qty.value;
}
onSendRequestDetail = function (form) {
    let url = '/Order/Create';
    let request = serializeForm(form);
    let xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);
    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
    xhr.onload = function (e) {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                onRenderRequestDetail(xhr.responseText);
            } else {
                console.log("No se pudo conectar al servidor.");
            }
        }
    };
    xhr.onerror = function (ex) {
        console.log("Error", ex.message);
    };
    xhr.send(request);
};
onRenderRequestDetail = function (response) {    
    $("#createModal").modal('hide');
    let table = document.getElementById("orderDet");
    table.innerHTML = response;

    document.getElementById("add-btn").addEventListener('click', function (e) {        
        onSendRequest(form_create);
    });
    var elements = document.getElementsByClassName('remove-item-btn');

    Array.from(elements).forEach(function (item) {        
        item.addEventListener('click', function (e) {
            let orderId = this.getAttribute("data-orderId");
            let productId = this.getAttribute("data-productId");
            onDeleteItem(orderId, productId);
        });
    });
}

onDeleteItem = function (orderid, productid) {
    let url = `/Order/DeleteItem?orderId=${orderid}&productId=${productid}`;
    let xhr = new XMLHttpRequest();
    xhr.open("POST", url, true);    
    xhr.onload = function (e) {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {                
                onRenderRequestDetail(xhr.responseText);
            } else {
                console.log("No se pudo conectar al servidor.");
            }
        }
    };
    xhr.onerror = function (ex) {
        console.log("Error", ex.message);
    };
    xhr.send(null);
};