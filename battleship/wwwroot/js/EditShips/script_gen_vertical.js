function fleetPreparationLocation() {
	const fleetPreparationId = "fleet_preparation";
	var elFleetPreparation = document.getElementById(fleetPreparationId);
	
	if (elFleetPreparation.offsetWidth < elFleetPreparation.offsetHeight)
	{
		elFleetPreparation.classList.toggle("flex-column", true);
		var battlefieldId = document.getElementById("place-battlefield");
		battlefieldId.style.width = "100%";
	}

}