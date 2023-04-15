function changeValueFee(obj) {
    
    let input_fee = document.getElementById("TransportFee");
    if (obj.value == "Offline")
        input_fee.value = "0";
    else if (obj.value == "GHTK")
        input_fee.value = "22000";
    else 
        input_fee.value = "20000";
}

function displayInfor(obj) {
        document.getElementById("infor-banking").style.display = "block";
}

function hideInfor(obj) {
    document.getElementById("infor-banking").style.display = "none";
}
