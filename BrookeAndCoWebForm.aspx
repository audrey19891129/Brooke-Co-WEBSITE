<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrookeAndCoWebForm.aspx.cs" Inherits="WebApplicationBrookeAndCo.BrookeAndCoWebForm" %>
<script runat="server">



    public void Check(object sender, EventArgs e)
    {
        if (login.Visible == false)
        {
			Console.WriteLine("not visible");
        }
    }

</script>


<!DOCTYPE html>
<html>
<head>
	<meta charset="UTF-8" />
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<meta name="google-signin-client_id" content="778803378153-dr7d54vbi2qua2udk9jabn0q91squ2gg.apps.googleusercontent.com">
	<meta http-equiv="X-UA-Compatible" content="IE=edge" />
	<link rel="stylesheet" type="text/css" href="/jqueryUI/jquery-ui.css">
	<link rel="stylesheet" type="text/css" href="/Website/style.css" />
	<script src="/Website/countries.js"></script>
	<title>Projet 1</title>
	<script src="https://www.paypal.com/sdk/js?client-id=AZPXTSbap596knasJ3epXRbo1hAd3lW909Tp-PUiH9-JXqHB1RTrMnzgA4lQG-IccW3nwKydU0NXEM80">
	</script>
</head>
<body>
	<div id="fb-root"></div>
	<script async defer crossorigin="anonymous" src="https://connect.facebook.net/en_GB/sdk.js#xfbml=1&version=v9.0&appId=2778884979045979&autoLogAppEvents=1" nonce="ikFSkar5"></script>
	<header>
		<div class="header">
			<h1>Entertainment for all... at half the price!</h1>
			<div id="searchBar" class="searchBar ui-widget">
                <div class="searchContainer">
                    <input id="tags">
					<img src="/Website/png/icons8_search_2.ico" class="searchBtn"/>
                </div>
			</div>
		</div>
		<div id="cartDiv">
			Cart:&nbsp;<span id="idItem">0</span>
		</div>
	</header>
	<nav id="NavElement">
		<div>
			<div id="miniNavigator" class="miniNavigator">
				&#9776;
				<ul id="navigator2"></ul>
			</div>
		</div>
		<ul id="navigator">
			<li id="booksNav" class="lvl1">
				Books
				<ul id="booksNavUl" class="subNav">
					<li class="lvl2"><a id="snLit" href="#literature">Literature</a></li>
					<li class="lvl2"><a id="snProg" href="#programming">Programming</a></li>
					<li class="lvl2"><a id="snGeo" href="#geography">Geography</a></li>
				</ul>
			</li>
			<li id="gamesNav" class="lvl1">
				Games
				<ul id="gamesNavUl" class="subNav">
					<li class="lvl2"><a id="snPS4" href="#ps4">PlayStation4</a></li>
					<li class="lvl2"><a id="snXbox" href="#xbox">Xbox 360</a></li>
					<li class="lvl2"><a id="snWii" href="#wii">Wii Nu</a></li>
				</ul>
			</li>
			<li id="moviesNav" class="lvl1">
				Movies
				<ul id="moviesNavUl" class="subNav">
					<li class="lvl2"><a id="snAct" href="#action">Action</a></li>
					<li class="lvl2"><a id="snCom" href="#comedy">Comedy</a></li>
					<li class="lvl2"><a id="snDra" href="#drama">Drama</a></li>
					<li class="lvl2"><a id="snSci" href="#scifi">Sci-Fi</a></li>
				</ul>
			</li>
			<li id="contactNav" class="lvl1">Contact us</li>
			<li id="logNav" class="lvl1">Log out</li>
			<li id="accountNav" class="lvl1">
				My Acount
				<ul id="accountNavUl" class="subNav">
					<li class="lvl2"><a id="snInfos">My Infos</a></li>
					<li class="lvl2"><a id="snOrders">Orders</a></li>
					<li class="lvl2"><a id="snPay">Payments</a></li>
					<li class="lvl2"><a id="snAddr">Addresses</a></li>
				</ul>
			</li>
		</ul>
	</nav>
	<div id="login" class="main full"  runat="server">
		<div id="logIn" class="LoginOuter">
			<div class="LoginPageHeader">
				<div class="b1" id="thLogin"><span>Log in</span></div>
				<div class="b2" id="thRegister"><span>Register</span></div>
			</div>
			<form id="LoginForm">
				<div class="LoginPageBody">
					<div id="divTdRegist">
						<div><label>Last name : </label></div>
						<div class="regist"><input type="text" id="txtName" value="" title=""  pattern="[A-Za-z -]{2,15}" /></div>
						<div><label>First name : </label></div>
						<div class="regist"><input type="text" id="txtFName" value=""  title="" pattern="[A-Za-z -]{2,15}" /></div>
						<div><label>E-mail : </label></div>
						<div class="regist"><input type="email" id="txtEmail"></div>
						<div><label>Date of Birth : </label></div>
						<div class="regist"><input type="date" id="datepicker"></div>
						<div><label>User name : </label></div>
						<div class="regist"><input type="text" id="txtUsername"  title="" pattern="[A-Za-z0-9 -@&$]{5,15}"></div>
						<div><label>Password : </label></div>
						<div class="regist"><input type="password" id="txtPassW"  title="" pattern="[A-Za-z0-9 -@&$]{8,15}"></div>
					</div>
					<div id="divTgLog">
						<div><label>Username : </label></div>
						<div id="b5"><input type="text" id="txtUsernameLogIn" title="" pattern="[A-Za-z0-9 -@&$]{5,15}" /></div>
						<div><label>Password : </label></div>
						<div id="b7"><input type="password" id="txtPassWLogIn" title=""  pattern="[A-Za-z0-9 -@&$]{5,15}" /></div>
					</div>
				</div>
				<div class="LoginPageFoot">
					<button class="logBtn" type="button" id="btnLogIn">Login</button>
					<button class="logBtn" type="button" id="btnValidate">Register</button>
					<fb:login-button scope="public_profile,email">
					</fb:login-button>
					<div id="my-signin2"></div>
				</div>
			</form>
		</div>
	</div>
	<div id="container">
		<div id="mainpage" class="main">
			<div id="mainpageTop"><h1 class="title">Upcoming products</h1></div>
			<div id="mainpageCarousel"><div id="resizable" class="ui-widget-content"></div></div>
			<div id="mainpageMessage"><div id="welcome"></div></div>
		</div>
		<div id="books" class="main ProductsPage"></div>
		<div id="games" class="main ProductsPage"></div>
		<div id="movies" class="main ProductsPage"></div>
		<div id="contact" class="main ">
			<div id="anchorTable"></div>
			<div id="anchorForm"></div>
			<div id="map">
				<div id="maparea">
					<div id="anchorMap"></div>
					<div id="floating-panel"></div>
					<div id="right-panel"></div>
				</div>
			</div>
		</div>
		<div id="cart" class="main"></div>
		<div id="itemsInfos" class="overlay">

		</div>
		<div id="logout" class="main"></div>
		<div id="dialog" class="main"></div>
		<div id="account" class="main">
			<div id="clientInfos" class="ProductsPage"></div>
		</div>
		<div id="checkout" class="main"></div>
	</div>
	<footer>
		This website was made and is intended for educational purposes by Audrey Lachapelle
	</footer>
	<script type="text/javascript" src="https://code.jquery.com/jquery-3.5.1.js"></script>
	<script type="text/javascript" src="/jqueryUI/jquery-ui.min.js"></script>
	<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAdRO5ZFd9gjeZDzJ8RrpOEMZ-z50IKsWQ"></script>
	<script src="/jqueryUI/jquery-ui.js"></script>
	<script src="https://apis.google.com/js/platform.js?onload=renderButton" async defer></script>
	<script type="text/javascript" src="/Website/scripts.js"></script>
	<script src="https://www.paypal.com/sdk/js?client-id=sb&currency=CAD" data-sdk-integration-source="button-factory"></script>
</body>
</html>