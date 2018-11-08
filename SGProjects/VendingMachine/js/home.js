$(document).ready(function(){
	
	loadProducts();
	
	$('#add-dollar-button').click(function (event){
		var currentAmount = $('#moneyEntered').val();
		if(currentAmount == ""){
			currentAmount = 0;
		}
		var newAmount = parseFloat(currentAmount) + 1;
		var amount = newAmount.toFixed(2);
		$.ajax({
			type: 'GET',
			url: 'http://localhost:8080/items',
			'dataType': 'json',
			success: function(newAmount){
				$('#moneyEntered').val(amount);
			}
		});
			
	});
	$('#add-quarter-button').click(function (event){
		var currentAmount = $('#moneyEntered').val();
		if(currentAmount == ""){
			currentAmount = 0;
		}
		var newAmount = parseFloat(currentAmount) + .25;
		var amount = newAmount.toFixed(2);
		$.ajax({
			type: 'GET',
			url: 'http://localhost:8080/items',
			'dataType': 'json',
			success: function(newAmount){
				$('#moneyEntered').val(amount);
			}
		});
			
	});		
	$('#add-dime-button').click(function (event){
		var currentAmount = $('#moneyEntered').val();
		if(currentAmount == ""){
			currentAmount = 0;
		}
		var newAmount = parseFloat(currentAmount) + .1;
		var amount = newAmount.toFixed(2);
		$.ajax({
			type: 'GET',
			url: 'http://localhost:8080/items',
			'dataType': 'json',
			success: function(newAmount){
				$('#moneyEntered').val(amount);
			}
		});
			
	});
	$('#add-nickel-button').click(function (event){
		var currentAmount = $('#moneyEntered').val();
		if(currentAmount == ""){
			currentAmount = 0;
		}
		var newAmount = parseFloat(currentAmount) + .05;
		var amount = newAmount.toFixed(2);
		$.ajax({
			type: 'GET',
			url: 'http://localhost:8080/items',
			'dataType': 'json',
			success: function(newAmount){
				$('#moneyEntered').val(amount);
			}
		});
			
	});
	
	$('#submit-request-button').click(function (event){
		
		if($('#moneyEntered').val() == '')
		{
			$('#moneyEntered').val(0);
		}
		
		if($('#selectItem').val() == '')
		{
			$('#displayResponse').val("Select an item");
		}
		
		else
		{
		$.ajax({
				type: 'GET',
				url: 'http://localhost:8080/money/' + $('#moneyEntered').val() + '/item/' + $('#selectItem').val(),
				success: function(data){
					$('#moneyEntered').val(0);
					$('#displayResponse').val("Item vended");
					// var getChange = JSON.parse(data.responseText);
					var listChange = '<p>';
					if(data.quarters > 0)
					{
						listChange += 'quarters: ' + data.quarters + '<br>';
					}
					if(data.dimes > 0)
					{
						listChange += 'dimes: ' + data.dimes + '<br>';
					}
					if(data.nickels > 0)
					{
						listChange += 'nickels: ' + data.nickels + '<br>'
					}
					if(data.pennies > 0)
					{
						listChange += 'pennies: ' + data.pennies + '<br>'
					}
					$('#displayChange').html(listChange);
					loadProducts();
				},
				error: function(data){
					var check = JSON.parse(data.responseText);
					$('#displayResponse').val(check.message);
				}
		});
		}
	});
})


function loadProducts(){
		
	var listProducts = $('#listProducts');
	
	$.ajax({
		type: 'GET',
		url: 'http://localhost:8080/items',
		success: function(data, status){
			$.each(data, function(index, product) {
				var productIndex = $('#product' + product.id);
				var row = '<p>';
					row += product.id + '<br>';
					row += product.name + '<br>';
					row += '$' + product.price.toFixed(2) + '<br>';
					row += 'Quantity left: ' + product.quantity + '<br>';
					row += '</p>';
				productIndex.html(row);
			});
		},
		error: function() {
				$('#errorMessages')
                .append($('<li>')
                .attr({class: 'list-group-item list-group-item-danger'})
                .text('Error calling web service.  Please try again later.'));
		}
	})
	
}

function GetItemNumber(id){
	var productNumber = id.substring(id.length - 1);
	$('#selectItem').val(productNumber);
}

	$('td').hover(
		//in function
		function(){
			$(this).css('background-color', 'lightgray');
		},
		//out function
		function(){
			$(this).css('background-color', '')
		}
	);