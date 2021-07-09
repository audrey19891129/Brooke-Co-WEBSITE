
//$(document).ready(function () {

	/*======================================================Initialisation======================================================*/

	localStorage.setItem("CartItems", "");
	localStorage.clickcount = 0;
	var currentUser;
	var advertisement = ["/Website/Banners/Banner1.jpg", "/Website/Banners/Banner2.jpg", "/Website/Banners/Banner3.jpg", "/Website/Banners/Banner4.jpg", "/Website/Banners/Banner5.jpg", "/Website/Banners/Banner6.jpg", "/Website/Banners/Banner7.jpg", "/Website/Banners/Banner8.jpg", "/Website/Banners/Banner9.jpg", "/Website/Banners/Banner10.jpg"];
	Init();
	var index = 0;
	var currentDelivery = new Delivery("", "", "", "", "", "", "", "");
	var currentTransaction = new Transaction();
	var currentMessage = new Message("", "", "", "", "", "", "", "");

var productTitles = [];

	$(function () {
		$("#datepicker").datepicker();
	});

	$(function () {
		$("#resizable").resizable();
	});

	$(function () {
		$(document).tooltip();
	});

function loginUser(EMAIL) {
	var data = "email=" + EMAIL;
	var user = AjaxCallGet("getClientByEmail", data);
	if (user.id != 0) {
		var cart = [];
		var orders = [];
		var allorders = user.orders;

		$(allorders).each(function (index, obj) {

			if (obj.status == "ongoing") {
				cart.push(obj);
			}
			else {
				orders.push(obj);
			}
		});
		var u = new User(user.id, user.name, user.lastname, user.bday, user.email, user.username, user.password, cart, user.adresses, orders);
		createUser(u);
		currentUser = u;
		$("#login").hide();
		var cart = currentUser.cart;
		readUserCartKey(cart);
		booksPage();
		gamesPage();
		moviesPage();
		
	}
	else
		alert("There is no user account associated to this email address.");
}

//======================GOOGLE============================================//
function onSuccess(googleUser) {
	console.log('Logged in as: ' + googleUser.getBasicProfile().getName());
	var EMAIL = googleUser.getBasicProfile().getEmail();
	loginUser(EMAIL);
}
function onFailure(error) {
	console.log(error);
}
function renderButton() {
	gapi.signin2.render('my-signin2', {
		'scope': 'profile email',
		'width': 40,
		'height': 40,
		'longtitle': false,
		'theme': 'dark',
		'onsuccess': onSuccess,
		'onfailure': onFailure
	});
}


//==========================FACEBOOK============================================//

window.fbAsyncInit = function () {
	FB.init({
		appId: '2778884979045979',
		xfbml: true,
		version: 'v9.0'
	});
	FB.AppEvents.logPageView();
	FB.Event.subscribe('auth.authResponseChange', function (response) {
		if (response.status === 'connected') {
			console.log("<br>Connected to Facebook");
			//SUCCESS
			FB.api('/me', { locale: 'tr_TR', fields: 'name, email' },
				function (response) {
					console.log(response.email);
					loginUser(response.email);
					
				}
			);
		}
		else if (response.status === 'not_authorized') {
			console.log("Failed to Connect");
		} else {
			console.log("Logged Out");
		}
	});
};
	/*======================================================Event listeners=====================================================*/

	$(":input").on("change", function (event) {
		event.stopPropagation();
		var regExp = $(this).attr("pattern");
		var reg = new RegExp(regExp);
		var value = $(this).val();

		if (!(reg.test(value)))
		$(this).css("border", "2px solid red");
		else
		$(this).css("border", "none");
	}); 

/*---------------------SearchBar-------------------------------------------*/

$("#tags").autocomplete({
	source: function (request, response) {
		var results = $.ui.autocomplete.filter(productTitles, request.term);
		response(results.slice(0, 10));
	}
});

$("#tags").on("click", function (event) {
	event.stopPropagation();
	productTitles = productTitles.sort();
	$("#tags").val("");
});

$(".searchBtn").on("click", function (event) {
	event.stopPropagation();
	
	if ($("#tags").val().trim() != "") {
		var title = $("#tags").val();
		var not = title.replace(/'/g, "`");
		alert(not);
		var data = "title=" + not;
		var obj = AjaxCallGet("getProductByTitle", data);
		var product = JSON.stringify(obj);
		if (obj.title != "") {
			closeOverlay();
			ShowOverlay(product);
		}
		else {
			$("#tags").val("Sorry, this keyword or title does not match any product");
        }
    }
});
	/*---------------------------Animates mini-menu----------------------------*/

	$("#miniNavigator").on("click", function () {

		if ($("#navigator2").is(":hidden"))
			$("#navigator2").show();
		else
			$("#navigator2").hide();
	});


	/*---------------------sets navigator on window resize----------------------------*/

	$(window).on("resize", function (event) {
		setMenu();
	});

	/*---------------------Carousel buttons functions----------------------------*/

	$("div").on("click", ".m1 , .m3", function (event) {
		event.stopPropagation();
		var v = $(this).attr("value");
		index += parseInt(v);
		showProduct();
	});

	$("div").on("click", ".dot", function (event) {
		event.stopPropagation();
		var v = $(this).attr("value");
		index = parseInt(v);
		showProduct();
	});


	/*---------------------Add product to cart----------------------------*/

	$("div").on("click", "#popUpAddBtn, .addToCart", function (event) {
		event.stopPropagation();
		var product = $(this).attr('pcode');
		var price = $(this).attr('price');
		var quant = $(this).parent().find("select").children("option:selected").val();
		AddToCart(product, quant);
		AddEntry(product, quant, price, 0);
	});

	/*---------------------Remove item from localStorageKey----------------------------*/

	$("div").on("click", ".removeItem", function (event) {
		var item = $(this).attr("value");
		RemoveItem(item);
		createBasket();
		var order = currentUser.cart[0];
		var orderId = order.order_id;
		data = "orderId=" + orderId + "&pcode=" + item;
		AjaxCallMod("deleteEntryFromOrder", data);
	});

	$("div").on("click", ".emptCart", function (event) {

		event.stopPropagation();
		var tab= currentUser.cart;
		var cart = tab[0];
		var orderId = cart.order_id;
		var data = "orderId=" + orderId;
		AjaxCallMod("deleteAllEntriesFromOrder", data);
		localStorage.removeItem("CartItems");
		localStorage.setItem("CartItems", "");
		createBasket();	
	});


	/*---------------------Modify items on localStorageKey----------------------------*/

	$("div").on("change", ".txtBoxQuant", function (event) {
		var quant = parseInt($(this).val());
		var item = $(this).attr("obj");
		var isInCart = 0;
		if (quant < 0)
			quant = 0;
		else if (quant > 100)
			quant = 100;
		ModifyCartKey(quant, item, isInCart);
		createBasket();
		AddEntry(item, quant, 0, 1);
	});

	/*---------------------Got to cartPage----------------------------*/

	$("#cartDiv").click(function (event) {
		event.stopPropagation();
		createBasket();
		goto("#cart");
	});

	/*---------------------Go to to main page (header click)----------------------------*/

	$("header").on("click", function (event) {
		event.stopPropagation();
		goto("#mainpage");
	});


	function goto(show) {

		var hide = $(".main");

		$(hide).each(function (index, object) {
			$(object).hide();
		});
		$(show).show();
		closeOverlay();
	}

	/*---------------------Go to books page----------------------------*/

	$("#booksNav").click(function (event) {
		event.preventDefault();
		if ($(event.target).is("li.lvl1")) {
			goto("#books");
			booksPage();
		}
	});


	$("#snLit").on("click", function (event) {
		var f1 = function () { booksPage(); }
		var f2 = function () { goto("#books"); }
		scrollTo(f1, f2, "#literature", "#snLit");
	});

	$("#snProg").on("click", function (event) {
		var f1 = function () { booksPage(); }
		var f2 = function () { goto("#books"); }
		scrollTo(f1, f2, "#programming", "#snProg");
	});

	$("#snGeo").on("click", function (event) {
		var f1 = function () { booksPage(); }
		var f2 = function () { goto("#books"); }
		scrollTo(f1, f2, "#geography", "#snGeo");
	});

	/*---------------------Go to games page----------------------------*/

	$("#gamesNav").click(function (event) {
		if ($(event.target).is("li.lvl1")) {
			goto("#games");
			gamesPage();
		}
	})

	$("#snWii").on("click", function (event) {
		var f1 = function () { gamesPage(); }
		var f2 = function () { goto("#games"); }
		scrollTo(f1, f2, "#wii", "#snWii");
	});

	$("#snXbox").on("click", function (event) {
		var f1 = function () { gamesPage(); }
		var f2 = function () { goto("#games"); }
		scrollTo(f1, f2, "#xbox", "#snXbox");
	});

	$("#snPS4").on("click", function (event) {
		var f1 = function () { gamesPage(); }
		var f2 = function () { goto("#games"); }
		scrollTo(f1, f2, "#ps4", "#snPS4");
	});

	//----------------------------------------------------------------------------
	/*---------------------Go to movies page----------------------------*/

	$("#moviesNav").click(function (event) {
		if ($(event.target).is("li.lvl1")) {
			goto("#movies");
			moviesPage();
		}
	})

	$("#snAct").on("click", function (event) {
		event.stopPropagation();
		var f1 = function () { moviesPage(); }
		var f2 = function () { goto("#movies"); }
		scrollTo(f1, f2, "#action", "#snAct");
	});

	$("#snDra").on("click", function (event) {
		event.stopPropagation();
		var f1 = function () { moviesPage(); }
		var f2 = function () { goto("#movies"); }
		scrollTo(f1, f2, "#drama", "#snDra");
	});

	$("#snCom").on("click", function (event) {
		event.stopPropagation();
		var f1 = function () { moviesPage(); }
		var f2 = function () { goto("#movies"); }
		scrollTo(f1, f2, "#comedy", "#snCom");
	});

	$("#snSci").on("click", function (event) {
		event.stopPropagation();
		var f1 = function () { moviesPage(); }
		var f2 = function () { goto("#movies"); }
		scrollTo(f1, f2, "#scifi", "#snSci");
	});
	//---------------------------------Go to My Account-------------------------------------------


	$("#snInfos").on("click", function (event) {
		event.stopPropagation();
		generateInfoForm();
		goto("#account");
	});

	$("#snPay").on("click", function (event) {
		event.stopPropagation();
		generateClientCards();
		goto("#account");
	});

	$("#snAddr").on("click", function (event) {
		event.stopPropagation();
		generateAddresses("#clientInfos");
		goto("#account");
	});

	$("#snOrders").on("click", function (event) {
		event.stopPropagation();
		generateOrders();
		goto("#account");
	});

	//---------------------------------Events Common-------------------------------------------

	$("div").on("click", ".unlockBtn", function (event) {
		event.stopPropagation();
		var addressBtm = $(this).parent();
		var body = $(addressBtm).prev()
		unlockAllFields(body);
	});

	$("div").on("click", ".editBtn", function (event) {
		event.stopPropagation();
		var child = $(this);
		var box = findInput(child);
		$(box).attr("disabled", false);
	});


	//---------------------------------Events belonging to AccountInfos-------------------------------------------


	$("div").on("click", ".saveInfosBtn", function (event) {
		event.stopPropagation();
		var accountId = $(this).attr("AccountId");
		var Btm = $(this).parent();
		var table = InfosCardInfos(accountId, Btm);
		var data = table[0];
		var fields = table[1];
		var username = fields[4];
		var password = fields[5];
		if (validateFields(fields)) {
			AjaxCallMod("modifyClient", data, username, password);
			generateInfoForm();
		}
		else {
			$("#clientInfos").prepend("<div class='wide addBtnBar'><span class='alert'>All fields must be correctly filled</span></div>");
        }
	
	});

	$("div").on("click", ".deleteInfosBtn", function (event) {
		event.stopPropagation();
		var accountId = $(this).attr("AccountId");
		AjaxCallMod("deleteClient", "id=" + accountId);

		var content = "Do you realy want to permanently delete your account?";

		$("#dialog").append(
			'<p><span class="ui-icon ui-icon-alert"></span>Are you sure you want to permanently delete your account?</p>'
		);

		var funct = function () {
			logout();
		}
		callDialog(content, funct);

	});

	$("div").on("click", ".cancelInfosBtn", function (event) {
		event.stopPropagation();
		generateInfoForm();
	});


	//---------------------------------Events belonging to Addresses-------------------------------------------

	$("div").on("click", ".saveBtn", function (event) {
		event.stopPropagation();
		var orderId = $(this).attr("orderId");
		var addressBtm = $(this).parent();
		var table = addressCardInfos(orderId, addressBtm);;
		var data = table[0];
		var fields = table[1];

		if (validateFields(fields)) {
			AjaxCallMod("modifyDeliveryAddress", data);
			generateAddresses("#clientInfos");
		}
		else {
			$("#clientInfos").prepend("<div class='wide addBtnBar'><span class='alert'>All fields must be correctly filled</span></div>");
		}
		
	});


	$("div").on("click", ".deleteBinBtn", function (event) {
		event.stopPropagation();
		var data = "id=" + $(this).attr("orderId");
		var content = "Do you realy want to permanently delete this address?";

		$("#dialog").append(
			'<p><span class="ui-icon ui-icon-alert"></span>Are you sure you want to permanently delete this address?</p>'
		);

		var funct = function () {
			AjaxCallMod("deleteDeliveryAddress", data);
			generateAddresses("#clientInfos");
		}
		callDialog(content, funct);
	});

	$("div").on("click", ".addBtn", function (event) {
		event.stopPropagation();
		var obj = "", index = "X";
		var card = createAddressCard(index, obj);
		$("#itemsInfos").append(card);
		var obj = $("#itemsInfos").find("#addressItem");
		var body = $(obj).find(".AddressBody");
		unlockAllFields(body);
		$("#itemsInfos").show();
		populateCountries("countryX", "stateX");
	});

	$("div").on("click", ".cancelBtn", function (event) {
		closeOverlay();
		generateAddresses("#clientInfos");
	});


	$("div").on("click", ".createBtn", function (event) {
		event.stopPropagation();
		var addressBtm = $(this).parent();
		var table = addressCardInfos(0, addressBtm);
		var data = table[0] + "&client_id=" + currentUser.id;
		var fields = table[1];

		if (validateFields(fields)) {

			var div = $(this).prev();
			if ($(div).is(":hidden")) {
				AjaxCallMod("createDeliveryAddress", data);
				
				var card = $("#otherAddress").find(".Address_item");
				$(card).addClass("selectedCard");
				var x = "clientId=" + currentUser.id;
				currentDelivery.address_id = AjaxCallGet("getDeliveryAdressesByClientId", x, "delivery_id");
			}
			else {
				AjaxCallMod("createDeliveryAddress", data);
				closeOverlay();
				generateAddresses("#clientInfos");
            }
			
		}
		else {
			var content = "All fields must be correctly filled";
			var funct = function () {}
			Alert(content, funct);
			$("#dialog").append(
				'<p><span class="ui-icon ui-icon-alert"></span>All fields must be correctly filled</p>'
			);
		}
	});
	//---------------------------------Events belonging to Payments-------------------------------------------


	$("div").on("click", ".cardSave", function (event) {
		event.stopPropagation();
		var par = $(this).parent();
		var cardBody = $(par).prev();
		var id = $(this).attr("objID");
		var mod = "mod";
		var table = clientCardInfos(cardBody, id, mod);
		var data = table[0];
		var fields = table[1];

		if (validateFields(fields)) {
			AjaxCallMod("modifyClientCard", data);
			generateClientCards();
		}
		else {
			$("#clientInfos").prepend("<div class='wide addBtnBar'><span class='alert'>All fields must be correctly filled</span></div>");
		}
	});


	$("div").on("click", ".createCardBtn", function (event) {
		event.stopPropagation();
		var par = $(this).parent();
		var cardBody = $(par).prev();
		var mod = "new";
		var table = clientCardInfos(cardBody, 0, mod);
		var data = table[0];
		var fields = table[1];

		if (validateFields(fields)) {

			var div = $(this).prev();
			var checkbox = $(div).find("input");

			if ($(checkbox).is(':checkbox')) {
				if ($(checkbox).prop("checked") == true) {
					var date = $(checkbox).attr("deleteOn");
					data += "&delete_on=" + date;
					AjaxCallMod("createTemporaryClientCard", data);
				}
				else {
					AjaxCallMod("createClientCard", data);

					data = "clientId=" + currentUser.id;
					var cards = AjaxCallGet("getCardsByClientId", data);
					var lastcard = $(cards).get(-1);
					currentTransaction.card_id = lastcard.card_id;
					if (currentDelivery.address_id != "" & currentDelivery.service_id != "") {
						$(".ProceedToPayment").show();
					}

                }
            }
			else {
				AjaxCallMod("createClientCard", data);
				closeOverlay();
				generateClientCards();
            }	
			var card = $("#otherCard").find(".card_item");
			$(card).addClass("selectedCard");

		}
		else {
			var content = "All fields must be correctly filled";

			var funct = function () {
			}
			Alert(content, funct);
			$("#dialog").append(
				'<p><span class="ui-icon ui-icon-alert"></span>All fields must be correctly filled</p>'
			);
		}

	});
	
	$("div").on("click", ".cardDelete", function (event) {
		event.stopPropagation();
		var par = $(this).parent();
		var cardBody = $(par).prev();
		var id = $(this).attr("objID");
		var mod = "del";
		var table = clientCardInfos(cardBody, id, mod);
		var data = table[0];

		var content = "Do you realy want to permanently delete this card?";

		$("#dialog").append(
			'<p><span class="ui-icon ui-icon-alert"></span>Are you sure you want to permanently delete this card?</p>'
		);

		var funct = function () {
			AjaxCallMod("deleteClientCard", data);
			generateClientCards();
		}
		callDialog(content, funct);
	});

	$("div").on("click", ".addCCardBtn", function (event) {
		event.stopPropagation();
		var obj = "", index = "X";
		var card = createClientCard(index, obj);
		$("#itemsInfos").append(card);
		var obj = $("#itemsInfos").find("#cardItem");
		var body = $(obj).find(".cardBody");
		unlockAllFields(body);
		$(".overlay").show();
	});

	$("div").on("click", ".cancelCardBtn", function (event) {
		closeOverlay();
		generateClientCards();
	});


	//---------------------------------Events belonging to past orders-------------------------------------------

	$("div").on("click", ".moreBtn", function (event) {
		event.stopPropagation();
		$(this).toggleClass("lessBtn");
		var parent = $(this).parent();
		var sibling = $(parent).prev();
		$(sibling).toggleClass("visible");
	});


	//-------------------------------------Events belonging to checkout----------------------------------------------------

	$("div").on("click", ".buyBtn", function (event) {
		event.stopPropagation();
		$("#checkout").html("");
		$("#checkout").append(

			'<div id="paymentMethod" class="zone">'+
			'<div class="tabs">' +
			'<div id="tab1" class="activetab tab" contentdiv=""><span id="sptab1">Use my card</span></div><div id="tab2" contentdiv="" class="inactivetab tab"><span id="sptab2">Use another card</span></div>' +
			'</div>' +
			'<div class="tabContentTop"><div class="addressesBtn prevbtn size40" section="" title="Previous section"></div><div class="addressesBtn nextbtn size40" section="" title="Next section"></div></div>' +
			'<div class="tabContent">' +
			'<div id="existingAddress" class="content section"></div>' +
			'<div id="otherAddress" class="content"></div>' +
			'<div id="deliveryService" class="content section"></div>' +
			'<div id="review" class="content section"></div>' +
			'<div id="existingCard" class="content section"></div>' +
			'<div id="otherCard" class="content">' +
			'</div>' +
			'</div>' +
			'<div class="tabContentBtm">' +
			'<div class="DirectPayment Checkout boxShadow">Use direct payment</div>' +
			'<div class="ProceedToPayment Checkout boxShadow">Place Order</div>'+
			'</div>' +
			'</div>' +
			'</div> ' +
			'</div>' 
		),

		
		appendClientCardTo("#existingCard");
		var newCard = createClientCard("N", "");
		$("#otherCard").append(newCard);
		var obj = $("#otherCard").find(".card_item");
		var body = $(obj).find(".cardBody");
		unlockAllFields(body);
		generateAddresses("#existingAddress");
		var address = createAddressCard("X", "");
		$("#otherAddress").append(address);
		var country = "countryX";
		var state = "stateX";
		populateCountries(country, state);
		obj = $("#otherAddress").find(".Address_item");
		body = $(obj).find(".AddressBody");
		var btm = $(obj).find(".AddressBtm");
		var cancel = $(btm).find(".cancelBtn");
		$(cancel).hide();
		unlockAllFields(body);
		appendDeliveryType();
		$(".prevbtn").hide();
		$(".prevbtn").attr("section", "existingCard");
		generateReview();

		goto("#checkout");
		content(".content", "#existingAddress");
		$(".nextbtn").attr("section", "existingAddress");
		$("#tab1").attr("contentdiv", "#existingAddress");
		$("#tab2").attr("contentdiv", "#otherAddress");
		$("#sptab1").text("Use existing address");
		$("#sptab2").text("Use another address");
		$(".ProceedToPayment").hide();
		$(".DirectPayment").hide();
	});


	$("div").on("click", "#tab1", function (event) {
		event.stopPropagation();
		toggleTabs($(this));
		var show = $(this).attr("contentdiv");
		content(".content", show);
	});


	$("div").on("click", "#tab2", function (event) {
		event.stopPropagation();
		toggleTabs($(this));
		var show = $(this).attr("contentdiv");
		content(".content", show);
	});


	$("div").on("click", ".payment", function (event) {
		event.stopPropagation();
		var children = $("#existingCard").find(".payment");
		selectItem(children, $(this));
		currentTransaction.card_id = $(this).attr("cardId");
		if (currentDelivery.address_id != "" & currentDelivery.service_id != "") {
			$(".ProceedToPayment").show();
        }
	});

	$("div").on("click", ".ProceedToPayment", function (event) {
		event.stopPropagation();
		var payment = paymentModel();
		var order = currentUser.cart[0];
		currentTransaction.status = payment.stat;
		currentTransaction.transaction_confirmation = payment.confirm;
		currentTransaction.validation = payment.val;
		currentTransaction.amount = (currentDelivery.total_cost).toFixed(2);
		currentTransaction.order_id = order.order_id;

		var d = new Date().toISOString();
		var date = d.split("T");
		currentTransaction.date = date[0];

		var data = "id=0&card_id=" + currentTransaction.card_id + "&transaction_confirmation=" + currentTransaction.transaction_confirmation + "&status=" + currentTransaction.status + "&validation=" + currentTransaction.validation + "&amount=" + currentTransaction.amount + "&order_id=" + currentDelivery.order_id + "&date=" + currentTransaction.date;
		var transaction = AjaxCallGet("createClientTransaction", data);
		currentTransaction.id = transaction.id;
		
		if (currentTransaction.status == "paid") {
			
			data = "order_id=" + currentDelivery.order_id + "&address_id=" + currentDelivery.address_id + "&service_id=" + currentDelivery.service_id + "&delivery_date=" + currentDelivery.delivery_date + "&total_cost=" + currentTransaction.amount;
			var delivery = AjaxCallGet("createShipment", data);
			modifyStock();
			data = "orderId=" + currentDelivery.order_id + "&date=" + currentTransaction.date;
			AjaxCallMod("modifyOrderStatus", data);
			alert("Your order has been sent");
			localStorage.setItem("CartItems", "");
			createBasket();
			goto("#cart");
			currentMessage.orderId = currentDelivery.order_id;
			var addresses = currentUser.delivery;

			$.each(addresses, function (index, obj) {
				if (obj.delivery_id == currentDelivery.address_id)
					currentMessage.delivAddress = obj;
			});
			data = "receiver=" + currentUser.email + "&orderId=" + currentMessage.orderId +
				"&subtotal=" + currentMessage.subtotal + "&GST=" + currentMessage.GST +
				"&QST=" + currentMessage.QST + "&total=" + currentMessage.total + "&delivFee=" + currentMessage.delivFee +
				"&delivDate=" + currentMessage.delivDate + "&delivAddress=" + currentDelivery.address_id;
			var mail = AjaxCallGet("sendEmail", data);

		}
		else
		alert("The transaction could not be completed because of the following error : " + payment.val);
	});

	

	$("div").on("click", ".service", function (event) {
		event.stopPropagation();
		if (currentDelivery.address_id != "") {
			var children = $("#deliveryService").find(".service");
			selectItem(children, $(this));
			var serviceId = $(this).attr("serviceId");
			currentDelivery.service_id = serviceId;
			setAttributes();
			var cost = currentDelivery.total_cost;
			var amount = parseFloat(cost).toFixed(2);
			$("#paypal-button-container").remove();
			$(".tabContentBtm").append('<div id="paypal-button-container"></div>');
			initPayPalButton(amount);
			$("#paypal-button-container").hide();
		}
		else
			alert("Please first select delivery address to ship to.")
	});

	$("div").on("click", ".delivery", function (event) {
		event.stopPropagation();
		var children = $("#existingAddress").find(".delivery");
		selectItem(children, $(this));
		currentDelivery.address_id = $(this).attr("delivery_id");
		if (currentDelivery.service_id !="")
		setAttributes();
	});

	

	$("div").on("click", ".nextbtn", function (event) {
		event.stopPropagation();
		var actual = "#" + $(this).attr("section");
		nextStep(actual);
	});

	$("div").on("click", ".DirectPayment", function (event) {
		initSections("existingCard");
		nextStep("#review");
	});

	$("div").on("click", ".prevbtn", function (event) {
		event.stopPropagation();
		var actual = "#" + $(this).attr("section");
		prevStep(actual);
	});
	/*---------------------------Go to contact page----------------------------*/

	$("#contactNav").click(function () {
		$("#inputEmail").attr("value", currentUser.email);
		goto("#contact");
	})


	/*---------------------------------events belonging to LogIn page------------------------------*/



	$("#btnLogIn").on("click", function (event) {
		//event.stopPropagation();
		var username = document.querySelector("#txtUsernameLogIn").value.trim(),
			password = document.querySelector("#txtPassWLogIn").value.trim();
		if (validateUserExist(username, password)) {
			$("#login").hide();
			var cart = currentUser.cart;
			readUserCartKey(cart);
			booksPage();
			gamesPage();
			moviesPage();
		}
		else {
			ErrMessage("This username and password combination does not match any user.");
		}
	});

	$("#btnValidate").on("click", function (event) {
		event.stopPropagation();
		var name = document.querySelector("#txtName").value.trim(),
			lname = document.querySelector("#txtFName").value.trim(),
			dob = document.querySelector("#datepicker").value.trim(),
			email = document.querySelector("#txtEmail").value.trim(),
			username = document.querySelector("#txtUsername").value.trim(),
			password = document.querySelector("#txtPassW").value.trim();

		if (validateInput(name, lname, dob, email, username, password)) {

			var data = "username=" + username;
			var exists = AjaxCallGet("CheckUserNameIsFree", data);
			if (exists == true) {
				message = "This username already exists.";
				ErrMessage(message);
			}
			else {
				data = "email=" + email + "&username=" + username + "&password=" + password + "&name=" + name + "&lastname=" + lname + "&bday=" + dob;
				var x = AjaxCallGet("createClient", data);
				//searchUser(username, password);
				$("#txtUsername").val(username);
				showLogInDiv();
				$("#txtUsernameLogIn").val(username);
				showLogInDiv();
			}
		}
	});

	$("#btnRegister").on("click", function (event) {
		showLogInDiv();
	});

	$("#thRegister").on("click", function (event) {
		showRegisterDiv()
	});

	$("#thLogin").on("click", function (event) {
		showLogInDiv();
	});

	/*----------------------------------Log out----------------------------------*/

	$("#logNav").click(function (event) {
		event.stopPropagation();
		closeOverlay();
		$("#logout").append(
			'<div id="LogOut">' +
			'<p>Are you sure you want to log out?</p>' +
			'</div>'
		);
		$(function () {
			$("#LogOut").dialog({
				dialogClass: "noclose dialog",
				resizable: false,
				modal: true,
				hide: "fade",
				title: "You are about to leave",
				buttons: [
					{
						text: "YES",
						click: function () {
							localStorage.removeItem("CartItems");
							localStorage.removeItem("user");
							reInit();
							$(this).dialog("close");
						}
					},
					{
						text: "NO",
						click: function () {
							$(this).dialog("close");
						}
					}
				]
			});
		});
	})

	/*---------------------Stop propagation of 'select' box----------------------------*/

	$("div").on("click", "select", function (event) {
		event.stopPropagation();
	});

	/*---------------------Show overlay div (full infos for cards)----------------------------*/

	$("div").on("click", ".item", function (event) {
		event.stopPropagation();
		var data = $(this).attr("obj");
		ShowOverlay(data);

	});

	/*---------------------Hide overlay div (full infos for cards)----------------------------*/

	$("div").on("click", ".popCloseBtn", function (event) {
		event.stopPropagation();
		closeOverlay();
	});

	
	$("#itemsInfos").on('click', function (event) {

		var container;

		if ($('#itemsInfos').find("#overlay").length > 0) {

			if (!$(event.target).closest('#overlay').length) {
				closeOverlay();
			}
		}
		else if ($('#itemsInfos').find("#addressItem").length > 0) {

			 container = $("#addressItem");

			if (!container.is(event.target) && container.has(event.target).length === 0)
				closeOverlay();
		}
		else {
			 container = $("#cardItem");

			if (!container.is(event.target) && container.has(event.target).length === 0)
				closeOverlay();
        }
	});
	
	/*---------------------Toggle directions panel----------------------------*/

	$("#maparea").on('click', function (event) {

		if ($("#right-panel").html() != "") {
			if (!$(event.target).closest('#right-panel').length) {
				$("#right-panel").toggle("slide");
				$("#right-panel").html("");
			}
		}
	});

	/*-------------------------clear Contact us text area------------------------------*/

	$("div").on("click", ".emailBtn", function (event) {
		event.stopPropagation();
		$('#txtaMessage').val('');
		submit();
	});

	//====================================================================================================================================================/
	//
	//																		FUNCTIONS TO CALL 
	//
	//====================================================================================================================================================/

function validateFields(fields) {

	var ok = 1;

	$(fields).each(function (index, obj) {
		if (obj == "" || obj == undefined)
			ok = 0;
	});

	if (ok == 1)
		return true;
	else
		return false;
}

function setAttributes() {

	if (currentDelivery.address_id != "") {
		var x = "id=" + currentDelivery.address_id;
		var objA = AjaxCallGet("getDeliveryAdressById", x, "address_id");
		var data = "id=" + currentDelivery.service_id;
		var obj = AjaxCallGet("getServiceById", data, "service_id");

		var days;
		if (objA.country == "Canada")
			days = obj.local_days;
		else
			days = obj.overseas_days;

		var date = new Date();
		var future = (new Date(date.setDate(date.getDate() + days)).toISOString()).split("T");
		currentDelivery.delivery_date = future[0];
		var subtotal = (obj.fixed_rate + currentDelivery.subtotal);
		var GST, QST, total;
		GST = (subtotal * 0.05);
		QST = (subtotal * 0.09975);
		total = (subtotal + GST + QST);
		currentDelivery.total_cost = total;
		$("#spService").text((obj.fixed_rate).toFixed(2));
		$("#spSubtotal").text(subtotal.toFixed(2));
		$("#spGST").text(GST.toFixed(2));
		$("#spQST").text(QST.toFixed(2));
		$("#spTotal").text(total.toFixed(2));
		$("#spExpected").text(currentDelivery.delivery_date);
		currentMessage.delivFee = (obj.fixed_rate).toFixed(2);
		currentMessage.delivDate = currentDelivery.delivery_date;
		currentMessage.GST = GST.toFixed(2);
		currentMessage.QST = QST.toFixed(2);
		currentMessage.subtotal = subtotal.toFixed(2);
		currentMessage.total = total.toFixed(2);
	}
}

function selectItem(children, selected) {

	$(children).each(function (index, obj) {
		if ($(obj).hasClass("selectedCard")) {
			$(obj).removeClass("selectedCard");
		}
	});

	$(selected).addClass("selectedCard");
}

function Delivery(id, order_id, address_id, service_id, confirmation_number, delivery_date, total_cost, subtotal) {
	this.id = 0;
	this.order_id = order_id;
	this.address_id = address_id;
	this.service_id = service_id;
	this.confirmation_number = confirmation_number;
	this.delivery_date = delivery_date;
	this.subtotal = subtotal;
	this.total_cost = total_cost;
}

function toggleTabs(selected) {

	var tabs = $(".tabs").find(".tab");

	$(tabs).each(function (index, tab) {

		if ($(selected).hasClass("inactivetab")) {
			$(tab).removeClass("activetab");
			$(tab).addClass("inactivetab");
		}
		else {
			$(tab).removeClass("inactivetab");
			$(tab).addClass("activetab");
		}
	});

	if ($(selected).hasClass("inactivetab")) {
		$(selected).removeClass("inactivetab");
		$(selected).addClass("activetab");
	}
	else {
		$(selected).removeClass("activetab");
		$(selected).addClass("inactivetab");
	}
}

function desactivateTab(selected) {
	if ($(selected).hasClass("activetab")) {
		$(selected).removeClass("activetab");
		$(selected).addClass("inactivetab");
	}
}

function activateTab(selected) {
	if ($(selected).hasClass("inactivetab")) {
		$(selected).removeClass("inactivetab");
		$(selected).addClass("activetab");
	}
}

function content(selClass, selected) {
	var cont = $(".tabContent").find(selClass);

	$(cont).each(function (index, div) {

		$(div).hide();
	});
	$(selected).show();
}

function nextStep(actual) {
	var next = $(actual).next();
	var section;

	if (!$(next).hasClass("section")) {
		section = $(next).next().attr("id");
	}
	else {
		section = $(next).attr("id");
	}
	var selected = "#" + section;
	content(".content", selected);

	initSections(section)
}

function prevStep(actual) {
	var prev = $(actual).prev();
	var section;

	if (!$(prev).hasClass("section")) {
		section = $(prev).prev().attr("id");
	}
	else {
		section = $(prev).attr("id");
	}
	var selected = "#" + section;
	content(".content", selected);
	initSections(section)
}

function generateReview() {

	var order = currentUser.cart[0];
	index = "X";

	$("#review").append(
		'<div class="Review_item">' +
		'<div class="OrderTop">ORDER REVIEW</div>' +
		'<div class="ReviewBody">' +
		'</div>' +
		'<div class="OrderBtm">' +
		'<div>Expected delivery date: <span id="spExpected"></span></div>' +
		'</div>' +
		'</div>	'
	);

	entries = order.entries;
	var AmountBtaxes = 0, GST, QST, total;

	$(entries).each(function (index, obj) {
		$.ajax({
			url: "https://localhost:44333/BrookeAndCoWebService.asmx/getProductByPcode",
			method: "post",
			data: "pcode=" + obj.pcode,
			dataType: "json",
			async: false,
			success: function (data) {
				AmountBtaxes += (obj.price * obj.quantity);
				$(".ReviewBody").append(
					'<div class="tableGrid6">' +
					'<div><img src="' + data.picture + '" class="mini"></div><div>' + data.title + '</div><div>' + obj.quantity + '</div><div>' + obj.price + ' $</div><div>' + (obj.price * obj.quantity).toFixed(2) + ' $</div>' +
					'</div>'
				);
			},
		});
	});
	$(".ReviewBody").append(
		'<div class="total">' +
		'<div>Delivery : <span id="spService"></span> $ <br/>Subtotal : <span id="spSubtotal"></span> $ <br/>GST : <span id="spGST"></span> $ <br/>QST : <span id="spQST"></span> $ <br/>Total : <span id="spTotal"></span> $ <br/></div>' +
		'</div>'
	);
	$("")

	currentDelivery.order_id = currentUser.cart[0].order_id;
	currentDelivery.subtotal = AmountBtaxes;

}

function initSections(section) {

	switch (section) {

		case "existingAddress":

			$("#tab2").show();
			$("#sptab1").text("Use existing address");
			$("#sptab2").text("Use another address");
			$("#tab1").attr("contentdiv", "#existingAddress");
			$("#tab2").attr("contentdiv", "#otherAddress");
			activateTab("#tab1");
			desactivateTab("#tab2");
			$(".prevbtn").attr("section", "existingAddress");
			$(".nextbtn").attr("section", "existingAddress");
			$(".nextbtn").show();
			$(".prevbtn").hide();
			$(".ProceedToPayment").hide();
			$("#paypal-button-container").hide();
			$(".DirectPayment").hide();
			break;

		case "deliveryService":
			$("#sptab1").text("Delivery type");
			$("#tab2").hide();
			$("#tab1").attr("contentdiv", "#deliveryService");
			activateTab("#tab1");
			desactivateTab("#tab2");
			$(".prevbtn").attr("section", "deliveryService");
			$(".nextbtn").attr("section", "deliveryService");
			$(".prevbtn").show();
			$(".nextbtn").show();
			$(".ProceedToPayment").hide();
			$("#paypal-button-container").hide();
			$(".DirectPayment").hide();
			break;

		case "review":
			$("#sptab1").text("Review");
			$("#tab2").hide();
			$("#tab1").attr("contentdiv", "#review");
			activateTab("#tab1");
			desactivateTab("#tab2");
			$(".prevbtn").attr("section", "review");
			$(".nextbtn").attr("section", "review");
			$(".prevbtn").show();
			$(".nextbtn").hide();
			$(".ProceedToPayment").hide();
			if (currentDelivery.service_id != "") {
				$("#paypal-button-container").show();
				$(".DirectPayment").show();
			}

			break;

		case "existingCard":
			if (currentDelivery.service_id == "")
				alert("Please select delivery service");
			if (currentDelivery.address_id == "")
				alert("Please select address to ship to");

			$("#sptab1").text("Use existing card");
			$("#sptab2").text("Use another card");

			$("#tab2").show;
			$("#tab1").attr("contentdiv", "#existingCard");
			$("#tab2").attr("contentdiv", "#otherCard");
			activateTab("#tab1");
			desactivateTab("#tab2");
			$("#tab2").show();

			$(".prevbtn").attr("section", "existingCard");
			$(".nextbtn").attr("section", "existingCard");
			$(".nextbtn").hide();
			$(".prevbtn").show();
			$("#paypal-button-container").hide();
			$(".DirectPayment").hide();
			break;
	}
}


function modifyStock() {
	var order = currentUser.cart[0];
	var entries = order.entries;

	$(entries).each(function (index, obj) {

		var data = "pcode=" + obj.pcode;
		var quant = obj.quantity;
		var stocks = AjaxCallGet("getStockInListByPcode", data, "stocksIn");
		var left = quant;
		var i = 0;
		data = "pcode=" + obj.pcode + "&quant=" + quant;
		var products = AjaxCallGet("modifyProductByPcode", data);

		while (left != 0 & i < stocks.length) {
			var stock = stocks[i];
			var leftQuant = stock.left_quant;

			if (quant <= leftQuant) {

				data = "stock_id=" + stock.id + "&transaction_id=" + currentTransaction.id + "&type=sold&quant=" + quant;
				AjaxCallMod("newStockOut", data);
				left = 0;
			}
			else {
				data = "stock_id=" + stock.id + "&transaction_id=" + currentTransaction.id + "&type=sold&quant=" + leftQuant;
				AjaxCallMod("newStockOut", data);
				left = left - leftQuant;
				i++;
			}
		}
	});
}

function Message(orderId, subtotal, GST, QST, total, delivFee, delivDate, delivAddress) {
	this.orderId = orderId;
	this.subtotal = subtotal;
	this.GST = GST;
	this.QST = QST;
	this.total = total;
	this.delivFee = delivFee;
	this.delivDate = delivDate;
	this.delivAddress = delivAddress;
}

function initPayPalButton(amount) {

	paypal.Buttons({

		createOrder: function (data, actions) {
			return actions.order.create({
				purchase_units: [{ "amount": { "currency_code": "CAD", "value": amount } }], application_context: {
					shipping_preference: 'NO_SHIPPING'
				}
			});
		},
		onApprove: function (data, actions) {
			return actions.order.capture().then(function (details) {
				alert('Your order has been sent');
				
				var purchase_units = details.purchase_units[0];
				var payments = purchase_units.payments;
				var captures = payments.captures[0];
				currentTransaction.transaction_confirmation = captures.id;

				var d = new Date().toISOString();
				var date = d.split("T");
				currentTransaction.date = date[0];

				var data = "id=0&card_id=0&transaction_confirmation=" + currentTransaction.transaction_confirmation + "&status=paid&validation=success&amount=" + amount + "&order_id=" + currentDelivery.order_id + "&date=" + currentTransaction.date;
				var transaction = AjaxCallGet("createClientTransaction", data);
				currentTransaction.id = transaction.id;
				data = "order_id=" + currentDelivery.order_id + "&address_id=" + currentDelivery.address_id + "&service_id=" + currentDelivery.service_id + "&delivery_date=" + currentDelivery.delivery_date + "&total_cost=" + amount;
				var delivery = AjaxCallGet("createShipment", data);
				modifyStock();
				data = "orderId=" + currentDelivery.order_id + "&date=" + currentTransaction.date;
				AjaxCallMod("modifyOrderStatus", data);
				localStorage.setItem("CartItems", "");
				createBasket();
				goto("#cart");
				currentMessage.orderId = currentDelivery.order_id;
				var addresses = currentUser.delivery;

				$.each(addresses, function (index, obj) {
					if (obj.delivery_id == currentDelivery.address_id)
						currentMessage.delivAddress = obj;
				});
				data = "receiver=" + currentUser.email + "&orderId=" + currentMessage.orderId +
					"&subtotal=" + currentMessage.subtotal + "&GST=" + currentMessage.GST +
					"&QST=" + currentMessage.QST + "&total=" + currentMessage.total + "&delivFee=" + currentMessage.delivFee +
					"&delivDate=" + currentMessage.delivDate + "&delivAddress=" + currentDelivery.address_id;
				var mail = AjaxCallGet("sendEmail", data);
				
			});
		},
		style: {
			shape: 'rect',
			color: 'gold',
			layout: 'horizontal',
			label: 'paypal'
		},
		onError: function (err) {
			alert(err);
		}
		}).render('#paypal-button-container');

}

function paymentModel() {
	var confirm = Math.floor(Math.random() * 10000001) + 100000;
	const s = ["paid", "unpaid"];
	const v = ["card could not be validated", "insufficient funds or credit limit exceeded"];
	const randomS = Math.floor(Math.random() * s.length);
	var stat = s[randomS], val;
	if (stat == "unpaid") {
		randomR = Math.floor(Math.random() * v.length);
		val = v[randomR];
	}
	else
		val = "success";

	return new Payment(confirm, stat, val);
}

function Payment(confirm, stat, val) {
	this.confirm = confirm;
	this.stat = stat;
	this.val = val;
}

function Transaction(id, card_id, transaction_confirmation, status, validation, amount, order_id, date) {
	this.id = id;
	this.card_id = card_id;
	this.transaction_confirmation = transaction_confirmation;
	this.status = status;
	this.validation = validation;
	this.amount = amount;
	this.order_id = order_id;
	this.date = date;
}
	//--------------------------------------------------------------------------------------
	//	name: clientCardInfos
	//	input:cardBody(html E), id(int), mod(string)
	//	how: retrives infos in element's fields
	//	why: send data to server
	//	output: data (string)
	//--------------------------------------------------------------------------------------

	function clientCardInfos(cardBody, id, mod) {
		var type, holdername, expiration, security_code, card_number;
		var clientId = currentUser.id;

		$(cardBody).children().each(function (index, dom) {

				var box = $(dom).find("input");

				if ($(box).is("input"))
					box = $(dom).find("input");
				else
					box = $(dom).find("select");

				var param = $(box).attr("paramType");

				switch (param) {
					case "type": type = $(box).val();
						break;
					case "holdername": holdername = ($(box).val()).toUpperCase();
						break;
					case "expiration": expiration = $(box).val();
						break;
					case "security_code": security_code = $(box).val();
						break;
					case "card_number": card_number = $(box).val();
						break;
				}
			});

		var data;

		if (mod == "mod")
			data = "id=" + id + "&holdername=" + holdername + "&expiration=" + expiration;
		else if (mod == "new")
			data = "id=" + 0 + "&client_id=" + clientId + "&type=" + type + "&holdername=" + holdername + "&expiration=" + expiration + "&security_code=" + security_code + "&card_number=" + card_number;
		else
			data = "id=" + id;

		var fields = [type, holdername, expiration, security_code, card_number];
		var table = [data, fields];
		return table;
	}

	//--------------------------------------------------------------------------------------
	//	name: findInput
	//	input: child(html E)
	//	how: gets value from found element
	//	why: 
	//	output: none
	//--------------------------------------------------------------------------------------


	function findInput(child) {
		var parent = $(child).parent();
		var sibling = $(parent).prev();
		var box = $(sibling).find("input");

		if ($(box).is("input"))
			box = $(sibling).find("input");
		else
			box = $(sibling).find("select");

		return box;
	}


	//--------------------------------------------------------------------------------------
	//	name: unlockAllFields
	//	input: addressBtm(html E)
	//	how: unlocks all input fields in addressBtm
	//	why: sets property disabled to false
	//	output: none
	//--------------------------------------------------------------------------------------

	function unlockAllFields(body) {

		$(body).children().each(function (index, elmnt) {

			$(elmnt).children().each(function (index, dom) {
				var box = $(dom).find("input");

				if ($(box).is("input"))
					box = $(dom).find("input");
				else
					box = $(dom).find("select");

				
				if (!$(box).prop("disabled")) {
					$(box).attr("disabled", true);
                }
					
				else if ($(box).attr("disabled"))
					$(box).attr("disabled", false);
			});
		});
	}

	//--------------------------------------------------------------------------------------
	//	name: logout
	//	input: none
	//	how: return to main page and show login form
	//	why: user has logged out
	//	output: none
	//--------------------------------------------------------------------------------------

	function logout() {
		goto("#mainpage");
		$("#login").show();
		localStorage.setItem("user", "");
	}

	//--------------------------------------------------------------------------------------
	//	name: generateInfoForm
	//	input: none
	//	how: retrieves and prints data from database
	//	why: show user's personnal infos
	//	output: none
	//--------------------------------------------------------------------------------------

	function generateInfoForm() {

		$("#clientInfos").html("");
		$("#clientInfos").append(

			'<div class="UserCard_item">' +
			'<div class="AddressTop">ACCOUNT INFORMATIONS</div>' +
			'<div class="AddressBody">' +
			'<div class="tableGrid4">' +
			'<div><label>Last name: </label></div>' +
			'<div><input propType="lname" class="wide" type="text" value="' + currentUser.lname + '" pattern="[A-Za-z -]{2,30}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>First name: </label></div>' +
			'<div><input propType="fname" class="wide" type="text" value="' + currentUser.name + '" pattern="[0-9]{1,15}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>E-mail: </label></div>' +
			'<div><input propType="email" class="wide" type="email" value="' + currentUser.email + '" pattern="[A-Za-z -0-9]{1,10}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>Date of birth: </label></div>' +
			'<div><input propType="dob" class="wide" type="date" value="' + currentUser.dob + '" pattern="[0-9]{1,15}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>User name: </label></div>' +
			'<div><input propType="username" class="wide" type="text" value="' + currentUser.username + '" pattern="[A-Za-z0-9 -@&$]{5,15}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>Password: </label></div>' +
			'<div><input propType="password" class="wide" type="password" value="' + currentUser.password + '" pattern="[A-Za-z0-9 -@&$]{8,15}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'</div>' +
			'<div class="AddressBtm">' +
			'<div class="addressesBtn saveInfosBtn size40" AccountId="' + currentUser.id + '"></div>' +
			'<div class="addressesBtn deleteInfosBtn size40" AccountId="' + currentUser.id + '"></div>' +
			'<div class="addressesBtn cancelInfosBtn size40"></div>' +
			'<div class="addressesBtn unlockBtn size40"></div>' +
			'</div>'
		);
	}

	//--------------------------------------------------------------------------------------
	//	name: InfosCardInfos
	//	input: accountId (int), addressBtm (html E)
	//	how: retrieves html object informations in input fields
	//	why: send info to database
	//	output: string
	//--------------------------------------------------------------------------------------

	function InfosCardInfos(accountId, addressBtm) {
		var Body = $(addressBtm).prev();
		var lname, fname, email, dob, username, password;

		$(Body).children().each(function (index, elmnt) {

			$(elmnt).children().each(function (index, dom) {
				var box = $(dom).find("input");

				if ($(box).is("input"))
					box = $(dom).find("input");
				else
					box = $(dom).find("select");

				var type = $(box).attr("propType");

				switch (type) {
					case "lname": lname = $(box).val();
						break;
					case "fname": fname = $(box).val();
						break;
					case "email": email = $(box).val();
						break;
					case "dob": dob = $(box).val();
						break;
					case "username": username = $(box).val();
						break;
					case "password": password = $(box).val();
						break;
				}
			});
		});

		var data = "id=" + accountId + "&lastname=" + lname + "&name=" + fname + "&email=" + email + "&bday=" + dob + "&username=" + username + "&password=" + password;
		var fields = [lname, fname, email, dob, username, password];
		var table = [data, fields];
		return table;
	}

	//--------------------------------------------------------------------------------------
	//	name: addressCardInfos
	//	input: orderId (int), addressBtm (html E)
	//	how: retrieves html object informations in input fields
	//	why: send info to database
	//	output: string
	//--------------------------------------------------------------------------------------

	function addressCardInfos(orderId, addressBtm) {
		var addressBody = $(addressBtm).prev();
		var country, province, city, street, civicnumber, appartment, zipcode;

		$(addressBody).children().each(function (index, elmnt) {

			$(elmnt).children().each(function (index, dom) {
				var box = $(dom).find("input");

				if ($(box).is("input"))
					box = $(dom).find("input");
				else
					box = $(dom).find("select");

				var type = $(box).attr("propType");

				switch (type) {
					case "country": country = $(box).val();
						break;
					case "province": province = $(box).val();
						break;
					case "city": city = $(box).val();
						break;
					case "street": street = $(box).val();
						break;
					case "civicnumber": civicnumber = $(box).val();
						break;
					case "appartment": appartment = $(box).val();
						break;
					case "zipcode": zipcode = $(box).val();
						break;
				}
			});
		});
		var data = "id=" + orderId + "&country=" + country + "&province=" + province + "&city=" + city + "&street=" + street + "&civicnumber=" + civicnumber + "&appartment=" + appartment + "&zipcode=" + zipcode;
		var fields = [country, province, city, street, civicnumber, appartment, zipcode];
		var table = [data, fields];
		return table;
	}

	//--------------------------------------------------------------------------------------
	//	name: AjaxCallMod
	//	input: service (string), data (string), username (string), password (string)
	//	how: calls asp.net services
	//	why: modify data on database
	//	output: none
	//--------------------------------------------------------------------------------------

	function AjaxCallMod(service, data, username, password) {
		$.ajax({
			url: "https://localhost:44333/BrookeAndCoWebService.asmx/" + service,
			method: "post",
			data: data,
			dataType: "json",
			async: false,
			success: function (data) {

				if (username == undefined && password == undefined) {
					username = currentUser.username;
					password = currentUser.password;
				}
				searchUser(username, password);
			}
		});
	}

	function AjaxCallGet(service, data, type) {

		var value = "";

		$.ajax({
			url: "https://localhost:44333/BrookeAndCoWebService.asmx/" + service,
			method: "post",
			data: data,
			dataType: "json",
			async: false,
			success: function (tab) {
				
				var lastItem = tab[tab.length - 1];

				if (type == "delivery_id")
					value = lastItem.delivery_id;
				else
					value = tab;	
			},
			fail: function (xhr, textStatus, errorThrown) {
				alert('request failed');
			}
		});
		
		return value;
	}

	//--------------------------------------------------------------------------------------
	//	name: callDialog
	//	input: content (string), func (function)
	//	how: customizes alert 
	//	why: validate users actions
	//	output: none
	//--------------------------------------------------------------------------------------

	function callDialog(content, funct) {
		$(function () {
			$("#dialog").html("");

			$("#dialog").dialog({
				dialogClass: "dialog",
				resizable: false,
				modal: true,
				hide: "fade",
				title: "Confirmation",
				content: content,
				buttons: [
					{
						text: "YES",
						click: function () {
							funct();
							$(this).dialog("close");
						}
					},
					{
						text: "NO",
						click: function () {
							$(this).dialog("close");
						}
					}
				]
			});
		});
	}

	function Alert(content, funct) {
		$(function () {
			$("#dialog").html("");

			$("#dialog").dialog({
				dialogClass: "dialog",
				resizable: false,
				modal: true,
				hide: "fade",
				title: "Confirmation",
				content: content,
				buttons: [
					{
						text: "OK",
						click: function () {
							funct();
							$(this).dialog("close");
						}
					}
				]
			});
		});
	}


	//--------------------------------------------------------------------------------------
	//	name: generateOrders
	//	input: none
	//	how: extracts data from database and prints data
	//	why: show client's past orders
	//	output: none
	//--------------------------------------------------------------------------------------

	function generateOrders() {

		var orders = currentUser.orders;
		var status;
		var deliveryDate;
		var del = "";
		$("#clientInfos").html("");

		if (orders.length == 0)
			$("#clientInfos").append("<div>There are no past orders associated to this account</div>");
		else {
			$(orders).each(function (index, order) {

				var deliveries = order.deliveries;

				if (deliveries != "" || deliveries[0] != undefined) {
					var delivery = $(deliveries).get(-1);
					status = delivery.status;
					deliveryDate = delivery.delivery_date;
					del = delivery;
				}
				else {
					status = order.status;
					deliveryDate = "";
				}

				$("#clientInfos").append(
					'<div class="Order_item">' +
					'<div class="OrderTop">ORDER NUMBER :  <span>' + order.order_id + '</span></div>' +
					'<div class="OrderBody">' +
					'<div class="outerGrid">' +
					'<div><label>Order date: </label></div>' +
					'<div class="regist"><span>' + order.order_date + '</span></div>' +
					'<div><label>Status: </label></div>' +
					'<div class="regist"><span>' + status + '</span></div>' +
					'<div><label>Delivery date: </label></div>' +
					'<div class="regist"><span>' + deliveryDate + '</span></div>' +
					'</div>' +
					'</div>' +
					'<div class="orderDetails" id="OD' + index + '"></div>' +
					'<div class="OrderBtm">' +
					'<div class="addressesBtn moreBtn"></div>' +
					'</div>' +
					'</div>	'
				);

				entries = order.entries;
				var appendId = "#OD" + index;

				$(entries).each(function (index, obj) {
					$.ajax({
						url: "https://localhost:44333/BrookeAndCoWebService.asmx/getProductByPcode",
						method: "post",
						data: "pcode=" + obj.pcode,
						dataType: "json",
						async: false,
						success: function (data) {

							$(appendId).append(
								'<div class="tableGrid">' +
								'<div><img src="' + data.picture + '" class="mini"></div><div>' + data.title + '</div><div>' + obj.quantity + '</div><div>' + obj.price + ' $</div>' +
								'</div>'
							);
						},
					});

				});

				var deliveryname="";

				if (del != "") {
					
					$.ajax({
						url: "https://localhost:44333/BrookeAndCoWebService.asmx/getServiceById",
						method: "post",
						data: "id=" + del.service_id,
						dataType: "json",
						async: false,
						success: function (data) {
							deliveryname = data.name;
						},
					});
				}

				$(appendId).append(
					'<div class="total">' +
					'<div>Delivery :' + deliveryname + '<br/>Total : ' + (del.total_cost).toFixed(2) + ' $</div>' +
					'</div>'
				);
			});
		}
	}

	//--------------------------------------------------------------------------------------
	//	name: createAddressCard
	//	input: index, obj
	//	how: printable string used by generateAddresses();
	//	why: show client's payment cards
	//	output: string
	//--------------------------------------------------------------------------------------
	function deliveryCard(index, obj) {

		card = 
			'<div id="addressItem" class="Address_item delivery" delivery_id="' + obj.delivery_id +'">' +
			'<div class="AddressTop">ADDRESS <span>' + (index + 1) + '</span></div>' +
			'<div class="AddressBody">' +
			'<div class="tableGrid3">' +
			'<div><label>Street: </label></div>' +
			'<div><input propType="street" class="wide" type="text" value="' + obj.street + '" pattern="[A-Za-z -]{2,30}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Civic number: </label></div>' +
			'<div><input propType="civicnumber" class="wide" type="number" value="' + obj.civicnumber + '" pattern="[0-9]{1,15}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Appartement: </label></div>' +
			'<div><input propType="appartment" class="wide" type="text" value="' + obj.appartment + '"  class="wide" disabled /></div>' +
			'<div class="btn">' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>City: </label></div>' +
			'<div><input propType="city" class="wide" type="text" value="' + obj.city + '" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Zip Code: </label></div>' +
			'<div><input propType="zipcode" class="wide" type="text" value="' + obj.zipcode + '" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Country: </label></div>' +
			'<div><select propType="country" class="wide" id="country' + index + '" disabled></select></div>' +
			'<div class="btn">' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Province: </label></div>' +
			'<div><select propType="province" class="wide" id="state' + index + '" disabled></select></div>' +
			'<div class="btn">' +
			'</div>' +
			'</div>' +
			'</div>' +
			'<div class="AddressBtm">' +
			'</div>';
		return card;
	}

	function createAddressCard(index, obj) {
		var city = "", street = "", civicnumber = "", app = "", zipcode = "", deliveryId = "", number = "NEW";
		var bottom;
		if (obj != "") {
			city = obj.city;
			street = obj.street;
			civicnumber = obj.civicnumber;
			app = obj.appartment;
			zipcode = obj.zipcode;
			deliveryId = obj.delivery_id;
			number = (index + 1);

			bottom =
				'<div class="AddressBtm">' +
				'<div class="addressesBtn deleteBinBtn size40" orderId="' + deliveryId + '"></div>' +
				'<div class="addressesBtn saveBtn size40" orderId="' + deliveryId + '"></div>' +
				'<div class="addressesBtn unlockBtn size40"></div>' +
				'</div>';
		}
		else {
			bottom =
				'<div class="AddressBtm">' +
				'<div class="addressesBtn cancelBtn size40"></div>' +
				'<div class="addressesBtn createBtn size40"></div>' +
				'</div>';
		}

		var body =
			'<div id="addressItem" class="Address_item" delivery_id="' + deliveryId +'">' +
			'<div class="AddressTop">ADDRESS <span>' + number + '</span></div>' +
			'<div class="AddressBody">' +
			'<div class="tableGrid3">' +
			'<div><label>Street: </label></div>' +
			'<div><input propType="street" class="wide" type="text" value="' + street + '" pattern="[A-Za-z -]{2,30}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Civic number: </label></div>' +
			'<div><input propType="civicnumber" class="wide" type="number" value="' + civicnumber + '" pattern="[0-9]{1,15}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Appartement: </label></div>' +
			'<div><input propType="appartment" class="wide" type="text" value="' + app + '" pattern="[A-Za-z -0-9]{1,10}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>City: </label></div>' +
			'<div><input propType="city" class="wide" type="text" value="' + city + '" pattern="[A-Za-z -0-9]{1,10}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Zip Code: </label></div>' +
			'<div><input propType="zipcode" class="wide" type="text" value="' + zipcode + '" pattern="[A-Za-z -0-9]{1,10}" class="wide" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Country: </label></div>' +
			'<div><select propType="country" class="wide" id="country' + index + '" disabled></select></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid3">' +
			'<div><label>Province: </label></div>' +
			'<div><select propType="province" class="wide" id="state' + index + '" disabled></select></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'</div>';

		return body + bottom;
	}


	//--------------------------------------------------------------------------------------
	//	name: generateAddresses
	//	input: none
	//	how: extracts data from database and send data to print
	//	why: show client's payment cards
	//	output: none
	//--------------------------------------------------------------------------------------


	function generateAddresses(div) {
		$("#clientInfos").html("");
		$("#clientInfos").append(
			'<div class="wide addBtnBar"><div class="addressesBtn addBtn size40"></div></div>'
		);
		var addresses = currentUser.delivery;

		$(addresses).each(function (index, obj) {
			var card;

			if (div == "#existingAddress") {
				card = deliveryCard(index, obj);
				//alert(JSON.stringify(obj));
			}
			else {
				card = createAddressCard(index, obj);
			}
			$(div).append(card);
			
			var country = "country" + index;
			var state = "state" + index;
			populateCountries(country, state);
			var c = "#" + country;
			var p = "#" + state;
			$(c).val(obj.country);
			var index = $(c).prop('selectedIndex');
			autoPopulateStates(index, state);
			$(p).val(obj.province);
		});
	}

	//--------------------------------------------------------------------------------------
	//	name: createClientCard
	//	input: index, obj
	//	how: printable string used by generateClientCards();
	//	why: show client's payment cards
	//	output: string
	//--------------------------------------------------------------------------------------

	function createPaymentCard(index, obj) {

			var str = obj.card_number;
			var res = str.substring(str.length - 4);

		var card =
			'<div id="cardItem" class="card_item payment" cardId="' + obj.card_id + '"> ' +
			'<div class="cardTop">CARD NUMBER *********<span>' + res + '</span></div>' +
			'<div class="cardBody">' +
			'<div class="tableGrid4">' +
			'<div><label>Card type: </label></div>' +
			'<div><span>' + (obj.type).toUpperCase() + '</span></div> ' +
			'<div></div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>Holder name: </label></div>' +
			'<div><span>' + (obj.holdername).toUpperCase() + '</span></div>' +
			'<div></div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>Expiration date: </label></div>' +
			'<div><span>' + obj.expiration +'</span></div>' +
			'<div></div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>CVC: </label></div>' +
			'<div><span>' + obj.security_code +'</span></div>' +
			'<div></div>' +
			'</div>'+
			'<div class="cardBtm"></div></div>';

		return card;
    }


	function createClientCard(index, obj) {

		var id = "NEW", cardnumber = "", securitycode = "", holdername = "", expiration = "", str = "", res = "";

		if (obj != "") {
			id = obj.card_id;
			cardnumber = obj.card_number;
			securitycode = obj.security_code;
			holdername = obj.holdername;
			expiration = obj.expiration;
			str = obj.card_number;
			res = str.substring(str.length - 4);
		}

		var body =
			'<div class="cardBody">' +
			'<div class="tableGrid4">' +
			'<div><label>Card type: </label></div>' +
			'<div><select paramType="type" class="wide" disabled id="options' + index + '">' +
			'<option value="" hidden>choose card type</option>' +
			'<option value="american express">AMERICAN EXPRESS</option>' +
			'<option value="mastercard">MASTERCARD</option>' +
			'<option value="visa">VISA</option>' +
			'</select>' +
			'</div> ' +
			'<div class="btn">' +
			'<div></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>Holder name: </label></div>' +
			'<div><input  class="wide"  paramType="holdername" type="text" value="' + holdername + '" pattern="[A-Za-z -]{2,15}" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>Expiration date: </label></div>' +
			'<div><input  class="wide"   paramType="expiration"  type="date" value="' + expiration + '" pattern="[A-Za-z -]{2,15}" disabled /></div>' +
			'<div class="btn">' +
			'<div class="addressesBtn editBtn size25"></div>' +
			'</div>' +
			'</div>' +
			'<div class="tableGrid4">' +
			'<div><label>CVC: </label></div>' +
			'<div><input class="wide" paramType="security_code"  type="text" value="' + securitycode + '" pattern="[0-9]{3,5}" disabled /></div>' +
			'<div class="btn">' +
			'<div></div>' +
			'</div>' +
			'</div>';

		if (obj != "") {
			body += '<div class="tableGrid4 hidden">';
		}
		else {
			body += '<div class="tableGrid4">';
		}

		body += '<div><label>Card number: </label></div>' +
			'<div><input  class="wide"   paramType="card_number"  type="text" value="' + cardnumber + '" disabled /></div>' +
			'<div class="btn">' +
			'<div></div>' +
			'</div>' +
			'</div>' +
			'</div>';

		var top = '<div id="cardItem" class="card_item" cardId="'+id+'">';

		if (obj != "")
			top += '<div class="cardTop">CARD NUMBER *********<span>' + res + '</span></div>';
		else
			top += '<div class="cardTop">NEW CARD</div>';


		var btm;

		if (obj != "") {
			btm =
				'<div class="cardBtm">' +
				'<div class="addressesBtn cardSave size40" objID="' + id + '"></div>' +
				'<div class="addressesBtn cardDelete size40" objID="' + id + '"></div>' +
				'</div>' +
				'</div >';
		}
		else if (index == "N") {

			var date = new Date();
			var future = (new Date(date.setDate(date.getDate() + 30)).toISOString()).split("T");
			btm =
				'<div class="cardBtm">' +
				'<div><input type="checkbox" id="noSave" deleteOn="' + future[0] +'" title="If you check this option, this card will be permanently deleted from our system in 30 days.">'+
				'<label for= "noSave" >Do not save</label></div>' +
				'<div class="addressesBtn createCardBtn size40"></div>' +
				'</div>' +
				'</div >';
		}
		else {
			btm =
				'<div class="cardBtm">' +
				'<div class="addressesBtn cancelCardBtn size40"></div>' +
				'<div class="addressesBtn createCardBtn size40"></div>' +
				'</div>' +
				'</div >';
        }

		return top + body + btm;
	}


	function appendDeliveryType() {
		$.ajax({
			url: "https://localhost:44333/BrookeAndCoWebService.asmx/getServicesList",
			method: "post",
			dataType: "json",
			async: false,
			success: function (data) {

				$(data).each(function (index, obj) {
					var card = createDeliveryType(index, obj);
					$("#deliveryService").append(card);
				});
			}
		});
	}
	function createDeliveryType(index, obj) {

		var card =
			'<div id="cardItem" class="card_item service" serviceId="' + obj.id + '"> ' +
			'<div class="cardTop"><span>' + (obj.name).toUpperCase() + '</span></div>' +
			'<div class="cardBody">' +
			'<div class="tableGrid5">' +
			'<div><label>Cost: </label></div>' +
			'<div><span>' + (obj.fixed_rate).toFixed(2) + ' $</span></div> ' +
			'</div>' +
			'<div class="tableGrid5">' +
			'<div><label>Delivery days (local): </label></div>' +
			'<div><span>' + obj.local_days + ' business days</span></div>' +
			'</div>' +
			'<div class="tableGrid5">' +
			'<div><label>Delivery days (overseas)</label></div>' +
			'<div><span>' + obj.overseas_days + ' business days</span></div>' +
			'</div>' +
			'<div class="cardBtm"></div></div>';

		return card;
	}

	//--------------------------------------------------------------------------------------
	//	name: generateClientCards
	//	input: none
	//	how: extracts data from database and send data to print
	//	why: show client's payment cards
	//	output: none
	//--------------------------------------------------------------------------------------

	function generateClientCards() {

		$("#clientInfos").html("");
		$("#clientInfos").append(
			'<div class="wide addBtnBar"><div class="addressesBtn addCCardBtn size40"></div></div>'
		);

		appendClientCardTo("#clientInfos");
	}

	function appendClientCardTo(div) {
		$.ajax({
			url: "https://localhost:44333/BrookeAndCoWebService.asmx/getCardsByClientId",
			method: "post",
			data: "clientId=" + currentUser.id,
			dataType: "json",
			async: false,
			success: function (data) {

				$(data).each(function (index, obj) {
					var card;

					if (div == "#clientInfos") {
						card = createClientCard(index, obj);
						$(div).append(card);
						var select = "#options" + index;
						$(select).val(obj.type);
                    }
					
					else if (div == "#existingCard")
					{
						card = createPaymentCard(index, obj);
						$(div).append(card);
                    }
				});
			}
		});
    }


	//--------------------------------------------------------------------------------------
	//	name: scrollTo
	//	input: function1, function2, name of division, name of li
	//	how: Scrolls to the desired section of page
	//	why: navigator redirection
	//	output: none
	//--------------------------------------------------------------------------------------
	function scrollTo(f1, f2, div, li) {

		event.stopPropagation();
		f2();
		f1();
		var loop = true;

		while (loop) {

			if ($(div).length) {
				loop = false;

				setTimeout(function () {

					$("html, body").animate({
						scrollTop: $($(li).attr("href")).offset().top
					}, 500);
				}, 1000);
			}
		}
	}


	//--------------------------------------------------------------------------------------
	//	name: Init
	//	input: none
	//	how: --
	//	why: Initialises the pages
	//	output: none
	//--------------------------------------------------------------------------------------

	function Init() {
		contactPage();
		mainPage();
		$(".overlay").hide();
		$("#mainpage").show();
		$("#books").hide();
		$("#games").hide();
		$("#movies").hide();
		$("#contact").hide();
		$("#miniNavigator").hide();
		$("#navigator2").hide();
		setMenu();
		logInPage();
	}


	//--------------------------------------------------------------------------------------
	//	name: reInit
	//	input: none
	//	how: --
	//	why: reinitialises the page on logOut 
	//	output: none
	//--------------------------------------------------------------------------------------
	function reInit() {
		goto("#login");
		mainPage();
		$("#mainpage").show();
		$("#txtUsernameLogIn").val("");
		$("#txtPassWLogIn").val("");
	}

	//--------------------------------------------------------------------------------------
	//	name: setMenu
	//	input: none
	//	how: Checks window width on resize (event call)
	//	why: initiates navigator bar: full size or mini-menu
	//	output: none
	//--------------------------------------------------------------------------------------

	function setMenu() {

		var w = parseInt($("nav").css("width"), 10);

		if (w < 750) {

			if ($("#navigator2").is(':empty')) {
				$("#navigator2").append($("#booksNav"), $("#gamesNav"), $("#moviesNav"), $("#contactNav"),
					$("#logNav"), $("#accountNav"));
				$("#booksNavUl").attr("class", "subNav2");
				$("#gamesNavUl").attr("class", "subNav2");
				$("#moviesNavUl").attr("class", "subNav2");
				$("#accountNavUl").attr("class", "subNav2");
				$("#miniNavigator").show();
				$("#miniNavigator").effect("highlight", { color: 'red' }, 500);
			}
		}
		else {
			$("#navigator").append($("#booksNav"), $("#gamesNav"), $("#moviesNav"), $("#contactNav"),
				$("#logNav"), $("#accountNav"));
			$("#booksNavUl").attr("class", "subNav");
			$("#gamesNavUl").attr("class", "subNav");
			$("#moviesNavUl").attr("class", "subNav");
			$("#accountNavUl").attr("class", "subNav");
			$("#miniNavigator").hide();

		}
	}

	//--------------------------------------------------------------------------------------
	//	name: ititCarousel
	//	input: none
	//	how: Extracts images from file, appends divisions to mainpage and setTimer function to body 
	//	why: Creates and initialises the carousel
	//	output: none
	//--------------------------------------------------------------------------------------
	function ititCarousel() {
		if ($("#resizable").html("")) {
			$("#resizable").append(
				'<div id="Carousel">' +
				'<div class="innerLayout">' +
				'<div class="m1" value="-1"><div></div><div class="arrow">&#9664;</div></div>' +
				'<div class="m2">' +
				'<div></div>' +
				'<div id="dotsHolder">' +
				'</div>' +
				'</div>' +
				'<div class="m3" value="1"><div></div><div class="arrow">&#9654;</div></div>' +
				'</div>' +
				'</div>'
			);

			var timer = setInterval(function () {
				index++;
				showProduct();
			}, 6000);

			$("#mainpage").append('<script>' + timer + '</script>');
			var dots = $(".dot");
			dots.eq(0).css("background-color", "white");

			$(advertisement).each(function (index, image) {
				$("#dotsHolder").append('<div class="dot" value="' + index + '"></div>');
			});
			var dots = $(".dot");
			dots.eq(0).css("background-color", "white");
		}
	}


	//--------------------------------------------------------------------------------------
	//	name: showProduct
	//	input: none
	//	how: sets the background image of the carousel division
	//	why: Animates Carousel
	//	output: none
	//--------------------------------------------------------------------------------------
	function showProduct() {

		if (index > advertisement.length - 1)
			index = 0;

		if (index < 0)
			index = advertisement.length - 1;

		var src = "url('" + advertisement[index] + "')";

		var dots = $(".dot");
		dots.each(function () { $(this).css("background-color", ""); });
		dots.eq(index).css("background-color", "white");
		$("#Carousel").css("background-image", src).fadeIn();
	}


	//--------------------------------------------------------------------------------------
	//	name: readUserCartKey
	//	input: (obj) cart
	//	how: reads the content the currentUser' s cart 
	//	why: to build the CartItems localStorage key
	//	output: none
	//--------------------------------------------------------------------------------------
	function readUserCartKey(cart) {
		// cart is []
		var strCartItems = "";
		var items = 0;

		if (cart[0] != undefined) {

			var c = cart[0];

			var table = c.entries;

			$(table).each(function (index, obj) {

				var item = table[index];
				items = items + item.quantity;
				strCartItems += item.pcode + "=" + item.quantity + ";";
			});
		}
		localStorage.setItem("CartItems", strCartItems);
		localStorage.clickcount = items;
		$("#idItem").empty();
		$("#idItem").html(localStorage.clickcount);
	}


	//--------------------------------------------------------------------------------------
	//	name: validateInput
	//	input: name, fname, dob, email, username, password
	//	how: check if input matches pattern
	//	why: to create user
	//	output: boolean
	//--------------------------------------------------------------------------------------
	function validateInput(name, fname, dob, email, username, password) {

		var message;
		var n = new RegExp('^[A-Za-z -]{2,15}$'),
			e = new RegExp('^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,50}$'),
			u = new RegExp('^[A-Za-z0-9._%+-@&$]{5,50}$'),
			p = new RegExp('^[A-Za-z0-9 -@&$]{8,15}$');

		if (!(n.test(name))) {
			message = "The last name must be at leat 2 characters long and be composed of letters only";
			ErrMessage(message);
			return false;
		}
		else if (!(n.test(fname))) {
			message = "The first name must be at leat 2 characters long and be composed of letters only";
			ErrMessage(message);
			return false;
		}
		else if (dob == "") {
			message = "Please enter a date of birth.";
			ErrMessage(message);
			return false;
		}
		
		else if (!(e.test(email))) {
			message = "Please enter a valid email.";
			ErrMessage(message);
			return false;
		}
	
		else if (!(u.test(username))) {
			message = "The username must be at leat 5 characters long without any spaces.";
			ErrMessage(message);
			return false;
		}
		else if (!(p.test(password))) {
			message = "The password must be at leat 8 characters long and be composed of letters and/or digits. Only the characters '-, @, &, $' are accepted.";
			ErrMessage(message);
			return false;
		}

		return true;
	}


	//--------------------------------------------------------------------------------------
	//	name: ErrMessage
	//	input: (string) message
	//	how: creates dialog box 
	//	why: shows error code for wrong input
	//	output: none
	//--------------------------------------------------------------------------------------	
	function ErrMessage(message) {
		$("#login").append(
			'<div id="wrongInput">' +
			'<p id="ErrCode">' + message + '</p>' +
			'</div>'
		);
		$(function () {
			$("#wrongInput").dialog({
				dialogClass: "noclose Errdialog",
				resizable: false,
				modal: true,
				hide: "fade",
				buttons: [
					{
						text: "OK",
						click: function () {
							$(this).dialog("close");
						}
					}
				]
			});
		});
	}


	//--------------------------------------------------------------------------------------
	//	name: submit
	//	input: none
	//	how: creates dialog box 
	//	why: confirms comment has been sent
	//	output: none
	//--------------------------------------------------------------------------------------	
	function submit() {
		$("#anchorForm").append(
			'<div id="confirmSubmit">' +
			'<p>Your question has been sent.<br> Thank you for contacting us!</p>' +
			'</div>'
		);
		$(function () {
			$("#confirmSubmit").dialog({
				dialogClass: "noclose dialog",
				resizable: false,
				modal: true,
				hide: "fade",
				buttons: [
					{
						text: "OK",
						click: function () {
							$(this).dialog("close");
						}
					}
				]
			});
		});
	}

	//--------------------------------------------------------------------------------------
	//	name: createUser
	//	input: (object) user
	//	how: creates localStorage key "user" with object as parameter
	//	why: initialises the current user
	//	output: none
	//--------------------------------------------------------------------------------------
function createUser(user) {
		localStorage.removeItem("user");
		localStorage.setItem("user", JSON.stringify(user));
	}


	//--------------------------------------------------------------------------------------
	//	name: searchUser
	//	input: username, password
	//	how: extracts user matching username and password
	//	why: validates weither user exists or needs created
	//	output: boolean
	//--------------------------------------------------------------------------------------
	
	function searchUser(username, password) {

		var cart = [];
		var orders = [];
		var ok = 0;

		$.ajax({
			url: "https://localhost:44333/BrookeAndCoWebService.asmx/validateClientExists",
			method: "post",
			data: "username=" + username.trim() + "&password=" + password.trim(),
			dataType: "json",
			async: false,
			success: function (data) {

				var user = data;
				var allorders = data.orders;

				if (user.id != 0) {

					$(allorders).each(function (index, obj) {
						if (obj.status == "ongoing") {
							cart.push(obj);
						}
						else {
							orders.push(obj);
						}
					});
					var u = new User(user.id, user.name, user.lastname, user.bday, user.email, user.username, user.password, cart, user.adresses, orders);
					createUser(u);
					currentUser = u;
					ok = 1;
				}
			}
		});
		return ok;
    }

	//--------------------------------------------------------------------------------------
	//	name: validateUserExist
	//	input: username, password
	//	how: checks if local key "user" exists. If not, checks if username and password matched any user in users.json file
	//	why: validates weither user exists or needs created
	//	output: boolean
	//--------------------------------------------------------------------------------------
	function validateUserExist(username, password) {

		var ok = 0;
		
		if (username.trim() == "" || password.trim() == "")
			return false;
		else
			var ok = searchUser(username, password);

		if (ok == 1)
			return true;

		else
			return false;
	}

	//--------------------------------------------------------------------------------------
	//	name: User *Constructor*
	//	input:name, lname, dob, email, username, password
	//	how: created object User
	//	why: creates new user
	//	output: --
	//--------------------------------------------------------------------------------------
	function User(id, name, lastname, bday, email, username, password, cart, delivery, orders) {
		this.id = id;
		this.name = name;
		this.lname = lastname;
		this.dob = bday;
		this.email = email;
		this.username = username;
		this.password = password;
		this.cart = cart;
		this.delivery = delivery;
		this.orders = orders;
	}

	//--------------------------------------------------------------------------------------
	//	name: closeOverlay
	//	input:none
	//	how: empties the anchor divisions of the overlay
	//	why: resets the overlay
	//	output: none
	//--------------------------------------------------------------------------------------
	function closeOverlay() {
		$("#itemsInfos").html("");
		$("#itemsInfos").hide();
	}

	//--------------------------------------------------------------------------------------
	//	name: ShowOverlay
	//	input: data (string) from event listener
	//	how: appends content to previously emtpy main division
	//	why: see full info on products cards
	//	output: none
	//--------------------------------------------------------------------------------------

	function ShowOverlay(data) {

		var obj = JSON.parse(data);

		$(".overlay").show();

		$("#itemsInfos").append(
			'<div id="overlay" class="">' +
			'<div class="infosOuter">' +
			'<div class="infosTop">' +
		'<div class="topBox" id="anchorImage"></div>' +
		'<div class="topBox" id="anchorSum"></div>' +
			'</div>' +
		'<div class="infosBtm" id="anchorInfos"></div' +
			'</div>' +
			'</div >'
		);


		$("#anchorSum").append(
			"<div class='popCloseBtn'>&#10005</div>" +
			obj.pcode + "<br>" +
			"Price : " + obj.price + " $<br>"
		);
		$("#anchorImage").append("<img class='popImg' src='" + obj.picture + "'/>");

		if (obj.type == "book") {

			$("#anchorInfos").append(

				"Title : " + obj.title.replace(/'/g, "`") + "<br>" +
				"Genre : " + obj.genre + "<br>" +
				"Author(s) : " + obj.authors + "<br>" +
				"Published : " + obj.pubDate + "<br>" +
				"Pub. company : " + obj.pubCo + "<br>"
			);
		}
		else if (obj.type == "game") {

			$("#anchorInfos").append(

				"Title : " + obj.title + "<br>" +
				"Genre : " + obj.genre + "<br>" +
				"Console : " + obj.category + "<br>" +
				"Released on : " + obj.reldate + "<br>" +
				"Company : " + obj.company + "<br>"
			);
		}
		else if (obj.type == "movie") {

			$("#anchorInfos").append(

				"Title : " + obj.title + "<br>" +
				"Genre : " + obj.genre + "<br>" +
				"Director : " + obj.director + "<br>" +
				"Released in : " + obj.relyear + "<br>" +
				"Lead actors : " + obj.actors + "<br>"
			);
		}
		$("#anchorSum").append("<div class='combo'><label>Quantity: </label><select class='selBox2' id='selBox2'>" +
			"<option value='1' selected>1</option>" +
			"<option value='2'>2</option>" +
			"<option value='3'>3</option>" +
			"<option value='4'>4</option>" +
			"<option value='5'>5</option>" +
			"</select>" +
			"<div id='popUpAddBtn' class='popUpAddBtn' price='" + obj.price + "' pcode = '" + obj.pcode + "' > Add to cart</div > " +
			"</div>"
		);
		if (obj.inventory <= 15) {
			$("#anchorSum").append("<p class='alert'><b>There are only " + obj.inventory + " left in stock.</b></p>");
		}

	}


	//--------------------------------------------------------------------------------------
	//	name: RemoveItem
	//	input: item (string) from event listener
	//	how: modifies localStorage key ("CartItems")
	//	why: Remove item from cart
	//	output: none
	//--------------------------------------------------------------------------------------
	function RemoveItem(item) {

		var strNewString = "";
		var tabItems = localStorage.getItem("CartItems").split(';');

		for (var index = 0; index < tabItems.length; index++) {

			var tab = tabItems[index].split("=");

			if (tabItems[index] != "") {
				var product = tab[0];

				if (tab[0] != item) {
					strNewString += tab[0] + "=" + tab[1] + ";";
				}
			}
		}

		localStorage.removeItem("CartItems");
		localStorage.setItem("CartItems", strNewString);
	};

	//--------------------------------------------------------------------------------------
	//	name: AddEntry
	//	input: product, quant, price
	//	how: modifies localStorage key ("CartItems")
	//	why: Add items to cart
	//	output: none
	//--------------------------------------------------------------------------------------

	function stringDate() {
		var d = new Date();
		var y = d.getFullYear();
		var M = d.getMonth();
		var day = d.getDay();
		var hour = d.getHours();
		var min = d.getMinutes();
		var sec = d.getSeconds();
		var orderDate = y + "-" + M + "-" + day + " " + hour + ":" + min + ":" + sec;
		return orderDate;
	}

	//--------------------------------------------------------------------------------------
	//	name: createOrder
	//	input: order_id, pcode
	//	how: modifies database
	//	why: Add items to cart
	//	output: none
	//--------------------------------------------------------------------------------------
	function createOrder() {

		var clientId = currentUser.id;
		var data = "clientId=" + clientId;
		AjaxCallMod("createNewOrder", data);
    }
	//--------------------------------------------------------------------------------------
	//	name: AddEntry
	//	input: order_id, pcode
	//	how: modifies database
	//	why: Add items to cart
	//	output: none
	//--------------------------------------------------------------------------------------

	function AddEntry(pcode, quant, price, basket) {

		if (currentUser.cart[0] == undefined || currentUser.cart[0] == "") {
			createOrder();
		}
		
		var order = currentUser.cart[0];
		orderId = order.order_id;
		var data = "orderId=" + orderId + "&pcode=" + pcode + "&quantity=" + quant + "&price=" + price + "&basket=" + basket;
		var service = "addEntryToOrder";
		AjaxCallMod(service, data);
	}

	//--------------------------------------------------------------------------------------
	//	name: AddToCart
	//	input: product, quant
	//	how: modifies localStorage key ("CartItems")
	//	why: Add items to cart
	//	output: none
	//--------------------------------------------------------------------------------------
	function AddToCart(product, quant) {

		var isInCart = 0;
		
		var strCartItems = localStorage.getItem("CartItems");

		if (strCartItems != "") {

			var tabItems = localStorage.getItem("CartItems").split(";");

			for (var index = 0; index < tabItems.length-1; index++) {

				var tabI = tabItems[index].split("=");

				if (tabI[0] == product) {
					isInCart = 1;
				}
			}
		}
		
		else
			localStorage.clickcount = 0;

		if (isInCart == 1) {
			ModifyCartKey(quant, product, isInCart);
		}
		else {

			strCartItems += product + "=" + quant + ";";
			localStorage.removeItem("CartItems");
			localStorage.setItem("CartItems", strCartItems);
			localStorage.clickcount = Number(localStorage.clickcount) + parseInt(quant);
			$("#idItem").empty();
			$("#idItem").html(localStorage.clickcount);
		}
		
	}

	//--------------------------------------------------------------------------------------
	//	name: ModifyCartKey
	//	input: product, quant
	//	how: modifies localStorage key ("CartItems")
	//	why: modifies quantity of each items in cart
	//	output: none
	//--------------------------------------------------------------------------------------
	function ModifyCartKey(quant, item, isInCart) {
	
		var items = 0;

		if (quant > 101)
			quant = 100;
		else if (quant < 0)
			quant = 0;

		var strNewString = "";

		if (localStorage.getItem("CartItems") != "") {
			var tabItems = localStorage.getItem("CartItems").split(";");

			for (var index = 0; index < tabItems.length; index++) {

				if (tabItems[index] != "") {

					var p = tabItems[index].split("=");

					if (p[0] != item) {
						strNewString += p[0] + "=" + p[1] + ";";
						items = items + parseInt(p[1], 10);

					}
					else if (p[0] == item) {

						var Q = parseInt(quant, 10);

						if (isInCart == 1) { // <--- prevents cart adding more than +1 on click  ***is going trough only if called from products page

							if (Q + parseInt(p[1], 10) < 101) {
								Q = parseInt(quant, 10) + parseInt(p[1], 10);
							}
							else {
								Q = 100;
							}
							strNewString += p[0] + "=" + Q + ";";
							items = items + Q;
						}
						else {
							strNewString += item + "=" + Q + ";";
							items = items + Q;
						}
					}
				}
			}
		}
		else {
			strNewString += item + "=" + quant + ";";
        }
		localStorage.removeItem("CartItems");
		localStorage.setItem("CartItems", strNewString);
		localStorage.clickcount = items;
		$("#idItem").empty();
		$("#idItem").html(localStorage.clickcount);

	}

	//--------------------------------------------------------------------------------------
	//	name: floatingPanel
	//	input: none
	//	how: loads the content of the floating panel
	//	why: Part of the map area initialisation
	//	output: none
	//--------------------------------------------------------------------------------------
	function floatingPanel() {
		$("#floating-panel").append(
			'<strong>Starting point:</strong>' +
			'<select id="start">' +
			'<option value="" selected>Current position</option>' +
			'<option value="715 Boulevard de la Grande-Allée, Boisbriand, QC J7G 1W4">Boisbriand</option>' +
			'<option value="3072 Boulevard Dagenais O, Laval, QC H7P 1T6">Laval</option>' +
			'<option value="10755 Rue de St.-Réal, Montréal, QC H3M 2Y5">Montreal (North)</option>' +
			'<option value="6361 Trans-Canada Hwy, Pointe-Claire, Quebec H9R 5A5">Montreal (West)</option>' +
			'</select>' +
			'<br>' +
			'<strong>Ending point :</strong>' +
			'<select id="end">' +
			'<option value="" selected disabled hidden>Choose store location</option>' +
			'<option value="715 Boulevard de la Grande-Allée, Boisbriand, QC J7G 1W4">Boisbriand</option>' +
			'<option value="3072 Boulevard Dagenais O, Laval, QC H7P 1T6">Laval</option>' +
			'<option value="10755 Rue de St.-Réal, Montréal, QC H3M 2Y5">Montreal (North)</option>' +
			'<option value="6361 Trans-Canada Hwy, Pointe-Claire, Quebec H9R 5A5">Montreal (West)</option>' +
			'</select>'
		);
	}

	//--------------------------------------------------------------------------------------
	//	name: anchorTable
	//	input: none
	//	how: loads the content of the anchorTable
	//	why: Part of the contacts page initialisation
	//	output: none
	//--------------------------------------------------------------------------------------		


	function anchorTable() {

		$("#anchorTable").append(
			"<div id='contactTable' class='table'>" +
			"<div>" +
			"<div class='row thead'>" +
			"<div class='rowItem2'>" +
			"Stores" +
			"</div>" +
			"<div class='rowItem2'>" +
			"Adress" +
			"</div>" +
			"<div class='rowItem2'>" +
			"Phone number" +
			"</div>" +
			"</div>" +
			"</div>" +
			"<div class='row'>" +
			"<div class='rowItem2'>Boisbriand</div>" +
			"<div class='rowItem2'>715 Boulevard de la Grande-Allée, <br>Boisbriand, QC J7G 1W4</div>" +
			"<div class='rowItem2'>450-568-7851</div>" +
			"</div>" +
			"<div class='row'>" +
			"<div class='rowItem2'>Laval</div>" +
			"<div class='rowItem2'>715 3072 Boulevard Dagenais O,  <br>Laval, QC H7P 1T6</div>" +
			"<div class='rowItem2'>450-258-7157</div>" +
			"</div>" +
			"<div class='row'>" +
			"<div class='rowItem2'>Montreal (North)</div>" +
			"<div class='rowItem2'>715 10755 Rue de St.-Réal,  <br>Montréal, QC H3M 2Y5</div>" +
			"<div class='rowItem2'>514-228-0579</div>" +
			"</div>" +
			"<div class='row'>" +
			"<div class='rowItem2'>Montreal (West)</div>" +
			"<div class='rowItem2'>6361 Trans-Canada Hwy,  <br>Pointe-Claire, Quebec H9R 5A5</div>" +
			"<div class='rowItem2'>438-547-1324</div>" +
			"</div>" +
			"</div>"
		);
	}


	//--------------------------------------------------------------------------------------
	//	name: anchorForm
	//	input: none
	//	how: loads the content of the anchorForm
	//	why: Part of the contacts page initialisation
	//	output: none
	//--------------------------------------------------------------------------------------				
	function anchorForm() {

		$("#anchorForm").append(
			'<div class="Email">' +
			'<label>E-mail : </label><input id="inputEmail" type="email" size="40"></input><br><br>' +
			'<label>What can we do for you?</label><br>' +
			'<textarea id="txtaMessage" rows="10" cols="40"  style="resize:none"></textarea><br>' +
			'<button type="button" class="emailBtn">Send</button>' +
			'</div>	'
		);
	}



	//--------------------------------------------------------------------------------------
	//	name: setPosition
	//	input: position (coordinates object from maps api)
	//	how: Creates map and sets points of interest
	//	why: initialises the map and directions from the google maps api
	//	output: none
	//--------------------------------------------------------------------------------------		 
	function setPosition(position) {
		var latitude = position.coords.latitude,
			longitude = position.coords.longitude;
		var directionsDisplay = new google.maps.DirectionsRenderer;
		var directionsService = new google.maps.DirectionsService;
		var centerGMap = new google.maps.LatLng(latitude, longitude);
		var optionsGMap = {
			zoom: 12,
			center: centerGMap,
			mapTypeId: google.maps.MapTypeId.ROADMAP
		};

		var Newmap = new google.maps.Map(document.querySelector("#anchorMap"), optionsGMap);
		directionsDisplay.setMap(Newmap);
		directionsDisplay.setPanel(document.querySelector("#right-panel"));

		var markUser = new google.maps.Marker({
			position: { lat: latitude, lng: longitude },
			map: Newmap,
			title: "You are here",
			icon: '/Website/png/star.png'
		});
		var commentUser = "<div style='color:teal;'><h1>You are here</h1></div>";
		var windowUser = new google.maps.InfoWindow({
			content: commentUser
		});
		google.maps.event.addListener(markUser, "click", function () {
			windowUser.open(Newmap, markUser);
		});

		var markBoisbriand = new google.maps.Marker({
			position: { lat: 45.609285, lng: -73.838117 },
			map: Newmap,
			title: "Brooke&Co - Boisbriand"
		});
		var commentBoisbriand = "<div style='color:teal;'><h1>Brooke&Co</h1></div>";
		var windowBoisbriand = new google.maps.InfoWindow({
			content: commentBoisbriand
		});
		google.maps.event.addListener(markBoisbriand, "click", function () {
			windowBoisbriand.open(Newmap, markBoisbriand);
		});

		var markLaval = new google.maps.Marker({
			position: { lat: 45.578736, lng: -73.791780 },
			map: Newmap,
			title: "Brooke&Co - Laval"
		});
		var commentLaval = "<div style='color:teal;'><h1>Brooke&Co</h1></div>";
		var windowLaval = new google.maps.InfoWindow({
			content: commentLaval
		});
		google.maps.event.addListener(markLaval, "click", function () {
			windowLaval.open(Newmap, markLaval);
		});

		var markMtlN = new google.maps.Marker({
			position: { lat: 45.538807, lng: -73.678627 },
			map: Newmap,
			title: "Brooke&Co - Montreal (North)"
		});
		var commentMtlN = "<div style='color:teal;'><h1>Brooke&Co</h1></div>";
		var windowMtlN = new google.maps.InfoWindow({
			content: commentMtlN
		});
		google.maps.event.addListener(markMtlN, "click", function () {
			windowMtlN.open(Newmap, markMtlN);
		});

		var markMtlW = new google.maps.Marker({
			position: { lat: 45.467215, lng: -73.823882 },
			map: Newmap,
			title: "Brooke&Co - Montreal (West)"
		});
		var commentMtlW = "<div style='color:teal;'><h1>Brooke&Co</h1></div>";
		var windowMtlW = new google.maps.InfoWindow({
			content: commentMtlW
		});
		google.maps.event.addListener(markMtlW, "click", function () {
			windowMtlW.open(Newmap, markMtlW);
		});

		var control = document.querySelector("#floating-panel");
		control.style.display = "block";
		Newmap.controls[google.maps.ControlPosition.TOP_CENTER].push(control);

		var onChangeHandler = function () {
			var startIndex = document.querySelector("#start").selectedIndex;
			var endIndex = document.querySelector("#end").selectedIndex;

			var startpoint = "";
			if (startIndex == 0) {
				startpoint = position.coords.latitude + ", " + position.coords.longitude;
			}
			else {
				startpoint = document.querySelector("#start").value;
			}
			var endingpoint = "";
			if (endIndex == 0) {
				endingpoint = startpoint;
			}
			else {
				endingpoint = document.querySelector("#end").value;
			}

			directionsService.route(
				{
					origin: startpoint,
					destination: endingpoint,
					travelMode: "DRIVING"
				}, function (response, status) {
					if (status === "OK") {
						$("#right-panel").toggle("slide");
						directionsDisplay.setDirections(response);
					}
					else {
						window.alert("Directions request failed due to " + status);
					}
				});
		}
		document.querySelector("#start").addEventListener("change", onChangeHandler);
		document.querySelector("#end").addEventListener("change", onChangeHandler);

	}


	//--------------------------------------------------------------------------------------
	//	name: initializeMap
	//	input: none
	//	how: hides the directions panel, and gets current position to send setPosition(coordinates)
	//	why: initialises the map with geolocalisation
	//	output: none
	//--------------------------------------------------------------------------------------	 
	function InitializeMap() {
		$("#right-panel").hide();
		if (!navigator.geolocation) {
			$("#anchorMap").html("GEO IS OFF");;
			return false;
		}
		else {
			navigator.geolocation.getCurrentPosition(setPosition);
		}
	}


	//=========================================================Pages Initialisation==============================================

	//--------------------------------------LogIn Page-----------------------------------------

	function logInPage() {
		$("#divTdRegist").hide();
		$("#btnRegister").hide();
		$("#btnValidate").hide();
	}


	function showLogInDiv() {
		$("#thLogin").css("background-color", "#858585");
		$("#thRegister").css("background-color", "#555");
		$("#thLogin").css("color", "white");
		$("#thRegister").css("color", "#858585");
		
		$("#divTgLog").show();
		$("#divTdRegist").hide();
		$("#btnValidate").hide();
		$("#btnLogIn").show();
	}


	function showRegisterDiv() {
		$("#thLogin").css("background-color", "#555");
		$("#thRegister").css("background-color", "#858585");
		$("#thLogin").css("color", "#858585");
		$("#thRegister").css("color", "white");

		$("#divTgLog").hide();
		$("#divTdRegist").show();
		$("#btnValidate").show();
		$("#btnLogIn").hide();
		
	}


	function AjaxCall(url, category, search) {

		$.ajax({
			url: url,
			method: "post",
			data: "category=" + search,
			async: false,
			dataType: "json",
			success: function (data) {

				var Table = data;
				
				$(Table).each(function (index, obj) {

					if (!(productTitles.includes(obj.title))) {
						productTitles.push(obj.title);
                    }
					

					var card = "<div class='item' obj='" + JSON.stringify(obj).replace(/'/g, "`") + "'>" +
							"<div class='innerTop'>" +
							"<img class='imageItem' src='" + obj.picture + "'/>" +
							"</div>" +
							"<div class='innerBtm'>" +
							"<div class='cartInnerTop'>" +
							"<div class='description'>" + obj.title + "</div>" +
							"</div>" +
							"<div class='cartInnerBtm'>" +
							"<div class='price'>" + (obj.price.toFixed(2)).toString() + " $</div>" +
							"<div class='quantity'><select class='selBox'>" +
							"<option value='1' selected>1</option>" +
							"<option value='2'>2</option>" +
							"<option value='3'>3</option>" +
							"<option value='4'>4</option>" +
							"<option value='5'>5</option>" +
							"</select>" +
							"</div>" +
							"</div>" +
							"</div>" +
						"<div class='addToCart' pcode='" + obj.pcode + "' price='" + obj.price + "'>Add to cart</div>" +
							"</div>" +
						"</div>";
					switch (category) {

						case "B-Prog": $("#programming").append(card);
							break;
						case "B-Geo": $("#geography").append(card);
							break;
						case "B-Lit": $("#literature").append(card);
							break;
						case "G-ps4": $("#ps4").append(card);
							break;
						case "G-wii": $("#wii").append(card);
							break;
						case "G-xbox": $("#xbox").append(card);
							break;
						case "M-act": $("#action").append(card);
							break;
						case "M-com": $("#comedy").append(card);
							break;
						case "M-dram": $("#drama").append(card);
							break;
						case "M-sci": $("#scifi").append(card);
							break;
                    }
				});
			},
			error: function (status) {
				alert("There was an error loading this page");
            }
		});
	}

//--------------------------------------Books-----------------------------------------
	function booksPage() {

		$("#books").append('<div id="programming"></div><div id="geography"></div><div id="literature"></div>');
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getBooksByCategory", "B-Prog", "programming");
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getBooksByCategory", "B-Geo", "geography");
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getBooksByCategory", "B-Lit", "literature");
	}
//--------------------------------------Games-----------------------------------------
	function gamesPage(){
		$("#games").append('<div id="wii"></div><div id="ps4"></div><div id="xbox"></div>');
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getGamesByCategory", "G-wii", "Wii U");
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getGamesByCategory", "G-ps4", 'PS4');
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getGamesByCategory", "G-xbox", 'Xbox One');
	}	

//--------------------------------------Movies-----------------------------------------
	function moviesPage() {
		$("#movies").append('<div id="action"></div><div id ="comedy"></div ><div id="drama"></div><div id="scifi"></div>');
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getMoviesByCategory", "M-act", 'Action / Adventure');
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getMoviesByCategory", "M-com", 'Comedy');
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getMoviesByCategory", "M-dram", 'Drama');
		AjaxCall("https://localhost:44333/BrookeAndCoWebService.asmx/getMoviesByCategory", "M-sci", 'Science-Fiction');
}	

//------------------------------------------Cart-------------------------------------------

	function createBasket(){

	   var AmountBtaxes = 0,
			items = 0,
			GST = 0,
			QST = 0,
			total = 0;
		var items = 0;
		var strCartItems = localStorage.getItem("CartItems");
		

		if(strCartItems == ""){
			$("#cart").html("<span class='empty'>Your cart is empty.</span>");	
			localStorage.clickcount = 0;
			$("#idItem").html(Number(localStorage.clickcount));
		}
		else{
				$("#cart").html("");	
				$("#cart").append(	
									"<div id='basketTable' class='table'>" +
										"<div id='header'>" +
											"<div class='row thead'>" +
												"<div class='rowItem'>" +
													"Product" +
												"</div>" +
												"<div class='rowItem'>" +
													"Description" +
												"</div>" +
												"<div class='rowItem'>" +
													"Price" +
												"</div>" +
												"<div class='rowItem'>" +
													"Quantity" +
												"</div>" +
												"<div class='rowItem'>" +
													"Subtotal" +
												"</div>" +
											"</div>" +
										"</div>" +
										"<div id='tableBody'>" +
										"</div>" +
										"<div id='tableFoot'>" +
										"</div>" +
									"</div>" 
									);

			var tabItems = localStorage.getItem("CartItems").split(";");
			
			
			for (var index = 0; index < tabItems.length-1; index++){

				var tab = tabItems[index].split("=");

				if(tabItems[index] != ""){

					$.ajax({
						url: "https://localhost:44333/BrookeAndCoWebService.asmx/getProductByPcode",
						method: "post",
						data: "pcode=" + tab[0],
						dataType: "json",
						async:false,
						success: function (data) {
							var p = data;

							AmountBtaxes += (p.price * parseInt(tab[1]));
							items += parseInt(tab[1]);
							GST = (AmountBtaxes * 0.05);
							QST = (AmountBtaxes * 0.09975);
							total = (AmountBtaxes + GST + QST);

							var newRow = "<div class='row'>" +
								"<div class='rowItem'>" +
								"<img src ='" + p.picture + "' class = 'img'>" +
								"</div>" +
								"<div class='rowItem'>" +
								p.title +
								"</div>" +
								"<div class='rowItem'>" +
								(p.price).toFixed(2) + " $" +
								"</div>" +
								"<div class='rowItem quantRow'>" +
								"<input class='txtBoxQuant' obj='" + p.pcode + "' type='number' min='0' max='100' value='" + tab[1] + "'></input>" +
								"<div class='removeItem' value='" + p.pcode + "'>&times;</div>" +
								"</div>" +
								"<div class='rowItem'>" +
								(p.price * parseInt(tab[1])).toFixed(2) + " $" +
								"</div>" +
								"</div>";

							$("#tableBody").append(newRow);
						},
						error: function (status) {
							alert("There was an error loading this page");
						}
					});
				}	
			}			
			var newFootRow =	"<div class='foot'>" + 
										"<div class='cartFoot'>Amount before taxes :  " + (AmountBtaxes).toFixed(2) + " $</div>" +
										"<div class='cartFoot'>GST :  " + (GST).toFixed(2) + " $</div>" +
										"<div class='cartFoot'>QST :  " + (QST).toFixed(2) + " $</div>" +
										"<div class='cartFoot'>TOTAL :  " + (total).toFixed(2) + " $</div>" +
										"<div class='cartFoot'>" +
											"<button type='button id='emptyCartBtn' class='emptCart'>Empty cart</button>" +
											"<button type='button id='buyBtn' var='2' class='buyBtn'>Buy</button>" +
										"</div>"+
								"</div>";
	
		$("#tableFoot").append(newFootRow);	
		localStorage.clickcount = parseInt(items);
		$("#idItem").html(localStorage.clickcount);
		}
	}


//------------------------------------------Contact Page-------------------------------------------

	function contactPage(){	
		floatingPanel();
		InitializeMap();	
		anchorForm();
		anchorTable();
	}


//------------------------------------------Main Page-------------------------------------------


	function mainPage(){

		ititCarousel();
		
		var welcome = "<h1>Who we are</h1>" +
					  "<p>mdfgjkhaugia rtioghf ghdfh gighirgh igiigyi hgk ot hir iihhthrgh iyerity yj uy uytutgyutuyuyu <br>" +
					  "fjf jfhjhv kfdhh  hh   kdfu efr khg rh gsd gu tf iwei b weut7et gudit u w76a ukt w 2kgz vab uk<br> " +
					  "mdfgjkhaugia rtioghf ghdfh gighirgh igiigyi hgk ot hir iihhthrgh iyerity yj uy uytutgyutuyuyu <br>" +
					  "fjf jfhjhv kfdhh  hh   kdfu efr khg rh gsd gu tf iwei b weut7et gudit u w76a ukt w 2kgz vab uk<br></p> " ;

		$("#welcome").html("");
		$("#welcome").append(welcome);
	}


//=================================================================================================================


