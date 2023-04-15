function eventDes() {
    let arrow_down = document.getElementsByClassName("fa-chevron-down").item(0);
    let arrow_up = document.getElementsByClassName("fa-chevron-up").item(0);
    let des_prod = document.getElementsByClassName("des-prod").item(0);

    if (des_prod.style.display === "none") {
        des_prod.style.display = "block";
        arrow_up.style.display = "inline";
        arrow_down.style.display = "none";
    }
    else {
        des_prod.style.display = "none";
        arrow_up.style.display = "none";
        arrow_down.style.display = "inline";
    }

}

function addToCart(endpoint, model) {
    let prod = JSON.parse(model);
    let quantity = document.getElementById("quantity").value;
    let data = JSON.stringify({
        "productId": parseInt(prod["id"]),
        "productName": String(prod["name"]),
        "price": parseFloat(prod["price"]),
        "quantity": parseInt(quantity)
    }); 

    endpoint = endpoint + String(prod["id"]);

    fetch(endpoint, {
        method: "POST",
        body: data,
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }
    }).then(res => {
        console.info(res);
        //console.info(prod);
        //console.info(data);
        alert("Vui lòng chuyển sang trang giỏ hàng để xem thông tin!");
        window.location.reload();
    });
}

function addComment(endpoint) {
    var formData = new FormData(document.querySelector(".cmt-fm"));
    fetch(endpoint, {
        method: "POST",
        body: formData
    }).then(res => {
        console.info(res);

        window.location.reload();
    })
}

function deleteComment(endpoint) {
    if (confirm("Bạn thực sự muốn xóa bình luận này?")) {
        fetch(endpoint, {
            method: "PATCH"
        }).then(res => {
            console.info(res);

            window.location.reload();
        })
    }
    
}