function editQuantity(endpoint, obj) {
    let quantity = obj.value;

    fetch(endpoint, {
        method: "PATCH",
        body: JSON.stringify({
            "quantity": parseInt(quantity)
        }),
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    }).then(res => {
        console.info(res);
        window.location.reload();
    })
}

function fmDeleteCart(productId) {
    document.getElementById("productId").value = productId;
}

function deleteCart(endpoint) {
    endpoint = endpoint + document.getElementById("productId").value;

    fetch(endpoint, {
        method: "DELETE"
    }).then(res => {
        console.info(res);
        window.location.reload();
    })
}