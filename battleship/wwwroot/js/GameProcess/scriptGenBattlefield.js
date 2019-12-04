function redrawBattlefield() {
	battlefieldLocation(); 
	genPlaceShipStat('player-ships-stat');
	genPlaceShipStat('opponent-ships-stat');
}

function battlefieldLocation() {
	const battlefieldId = "battlefield";
	var elParent = document.getElementById(battlefieldId);

	if (elParent.offsetWidth < elParent.offsetHeight) {
		elParent.classList.toggle("flex-column", true);
		document.querySelectorAll("#battlefield > div").forEach(function(el) {
			el.style.width = "100%";
			el.style.height = "50%";

		});

		var playerBattlefield = document.getElementById("player-battlefield");
		var opponentBattlefield = document.getElementById("opponent-battlefield");
		playerBattlefield.style.justifyContent = "center";
		playerBattlefield.style.display = "flex";
		opponentBattlefield.style.justifyContent = "center";
        opponentBattlefield.style.display = "flex";
        document.getElementById("opponent-ships-stat").parentElement.style.justifyContent = "flex-end";
        document.getElementById("opponent-battlefield").parentElement.style.justifyContent = "center";
	} else  {
		elParent.classList.toggle("flex-column", false);
	}

}