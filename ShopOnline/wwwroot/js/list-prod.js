//Gán giá trị cho edit form
function editFormProd(item) {
    let data = JSON.parse(item);
    console.info(data);
    document.getElementById("Id").value = data["id"];
    document.getElementById("Name").value = data["name"];
    document.getElementById("Description").value = data["description"];
    document.getElementById("CategoryId").value = String(data["categoryId"]);
    document.getElementById("Note").value = data["note"];
    document.getElementById("Price").value = String(data["price"]);
    document.getElementById("CreatedDate").value = String(data["createdDate"]);
    document.getElementById("View").value = String(data["view"]);
    if (Boolean(data["isActive"]) == true) {
        document.getElementById("IsActive").checked = true;
    }
    else {
        document.getElementById("IsActive").checked = false;
    }
    if (String(data["image"]) != "") {
        document.getElementById("prod-image").src = String(data["image"]);
        document.getElementById("note-image").innerHTML = "Ảnh đã được chọn và lưu trữ."
    }

}

//Update dữ liệu
function editProduct(endpoint) {
    endpoint = endpoint + document.getElementById("Id").value;
    /*let dataForm = {
        "Id": document.getElementById("Id").value,
        "Name": String(document.getElementById("Name").value),
        "Description": String(document.getElementById("Description").value),
        "CategoryId": document.getElementById("CategoryId").value,
        "Note": document.getElementById("Note").value,
        "Price": document.getElementById("Price").value,
        "CreatedDate": document.getElementById("CreatedDate").value,
        "View": document.getElementById("View").value,
        "Image": document.getElementById("Image").value
    };*/

    let dataForm = new FormData(document.querySelector(".fm-update-prod"));

    fetch(endpoint, {
        method: "PATCH",
        body: dataForm
    }).then(res => {
        console.info(res);

        window.location.reload();
    })

}

//Gán data vào delete form
function deleteFormProduct(id) {
    document.getElementById("del-prod").value = id;
}

//Delete sản phẩm
function deleteProduct(endpoint) {
    endpoint = endpoint + document.getElementById("del-prod").value;

    fetch(endpoint, {
        method: "DELETE"
    }).then(res => {
        console.info(res);

        window.location.reload();
    });
}

$(document).ready(function () {
    function Contains(text_one, text_two) {
        if (text_one.indexOf(text_two) != -1)
            return true;
    }
    $("#prod-search").keyup(function () {
        var searchText = $("#prod-search").val().toLowerCase();
        $(".tr-search").each(function () {
            if (!Contains($(this).text().toLowerCase(), searchText)) {
                $(this).hide();
            }
            else {
                $(this).show();
            }
        });
    });
});