window.onload = function () {
    let date = new Date();
    let yearNow = date.getFullYear();
    document.getElementById(`year_${yearNow}`).selected = "true";
}

function changeChartType() {
    let formData = new FormData(document.querySelector(".fm-report-prod"));
    let type = document.getElementById("ChartType").value;
    const ctx_1 = document.getElementById('prodChart_1');

    fetch("/Report/OrderRevenue", {
        method: "POST",
        body: formData,
        data: "",
        type: "application/json; charset=utf-8",
    }).then(res => res.json()).then(data => {
        let _chartLabels = data[0];
        let _chartData = data[1];

        console.info(_chartLabels);
        console.info(_chartData);

        new Chart(ctx_1, {
            type: `${type}`,
            data: {
                labels: _chartLabels,
                datasets: [{
                    label: 'Doanh Thu Của Cửa Hàng Trong Năm ' + formData.get("year"),
                    data: _chartData,
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    });

    document.getElementById("show-chart").style.display = 'none';
    document.getElementById("reset-chart").style.display = 'inline';
    document.getElementById("ChartType").disabled = 'true';
    document.getElementById("year").disabled = 'true';
}

function refreshPage() {
    window.location.reload();
    document.getElementById("show-chart").style.display = 'inline';
    document.getElementById("reset-chart").style.display = 'none';
    document.getElementById("ChartType").disabled = 'false';
    document.getElementById("year").disabled = 'false';
}