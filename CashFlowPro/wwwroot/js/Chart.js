window.createChart = (canvasId, labels, data) => {
    const ctx = document.getElementById(canvasId).getContext('2d');
    new Chart(ctx, {
        type: 'pie',  // Chart type set to 'pie'
        data: {
            labels: labels, // Labels like ['Debts', 'Income', 'Expenses']
            datasets: [{
                label: 'Amount',
                data: data, // Example data: [debts, income, expenses]
                backgroundColor: [
                    'rgba(75, 192, 192, 0.2)', // Color for Debts
                    'rgba(153, 102, 255, 0.2)', // Color for Income
                    'rgba(255, 159, 64, 0.2)' // Color for Expenses
                ],
                borderColor: [
                    'rgba(75, 192, 192, 1)', // Border color for Debts
                    'rgba(153, 102, 255, 1)', // Border color for Income
                    'rgba(255, 159, 64, 1)' // Border color for Expenses
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            plugins: {
                legend: {
                    position: 'top', // Position of legend (top, bottom, left, right)
                },
                tooltip: {
                    callbacks: {
                        label: (tooltipItem) => {
                            return `Amount: $${tooltipItem.raw}`; // Customize tooltip text
                        }
                    }
                }
            }
        }
    });

    window.onload = () => {
        const labels = ['Debts', 'Income', 'Expenses'];
        const data = [200, 500, 300]; // Example values
        window.createChart('transactionsPieChart', labels, data);
    };
};
