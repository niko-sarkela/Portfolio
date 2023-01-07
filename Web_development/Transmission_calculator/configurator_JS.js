
function CalculateWheel_d(w_width, w_profile, r_diameter) {
	return (r_diameter * 25.4) * (1 + w_profile / 100); //rim diameter (mm) + wheel profile in %	example: 15" * 25,4mm * 1,55 <--(+55%)
}

function HandleForm(){
	
	//Fetches wheels dimensions
	var tyreWidth = document.getElementById("tyreWidth").value;
	var tyreProfile = document.getElementById("tyreProfile").value;
	var rimDiameter = document.getElementById("rimDiameter").value;
	
	var gear1 = document.getElementById("g1").value;
	var gear2 = document.getElementById("g2").value;
	var gear3 = document.getElementById("g3").value;
	var gear4 = document.getElementById("g4").value;
	var gear5 = document.getElementById("g5").value;
	var gear6 = document.getElementById("g6").value;
	var finalDrive = document.getElementById("fd").value;
	var rpm = document.getElementById("rpm").value;
	var speed = document.getElementById("speed").value;
	
	var selectedGear = gear1;
	
	for (i = 1; i < 6; i++){
		if(document.getElementById("selectedGear" + i).checked){
			selectedGear = document.getElementById("g" + i).value;
		}
	}
	
	//alert("Renkaan läpimittä (mm): " + CalculateWheel_d(tyreWidth, tyreProfile, rimDiameter));
	document.getElementById("calculatedSpeed").innerHTML = "Speed: " + CalculateSpeed(selectedGear, finalDrive, rpm, CalculateWheel_d(tyreWidth, tyreProfile, rimDiameter)) + " km/h";
	document.getElementById("calculatedRPM").innerHTML = "RPM: " + CalculateRPM(selectedGear, finalDrive, speed, CalculateWheel_d(tyreWidth, tyreProfile, rimDiameter));
}

function CalculateSpeed(gear, fd, rpm, wheelDiameter) {
	return parseInt(wheelDiameter * Math.PI / 1000 / fd / gear * 60 * (rpm / 1000));
	
}
function CalculateRPM(gear, fd, speed, wheelDiameter) {
	return parseInt(25 / (wheelDiameter / 2  * Math.PI * 3) * speed * fd * gear * 1000);
	
}
function SelectTransmission(tableRow) {
	//alert(tableRow.parentElement.parentElement.getElementsByClassName("tableGear1st")[0].innerText);
	document.getElementById("g1").value = tableRow.parentElement.parentElement.getElementsByClassName("tableGear1st")[0].innerText;
	document.getElementById("g2").value = tableRow.parentElement.parentElement.getElementsByClassName("tableGear2nd")[0].innerText;
	document.getElementById("g3").value = tableRow.parentElement.parentElement.getElementsByClassName("tableGear3rd")[0].innerText;
	document.getElementById("g4").value = tableRow.parentElement.parentElement.getElementsByClassName("tableGear4th")[0].innerText;
	document.getElementById("g5").value = tableRow.parentElement.parentElement.getElementsByClassName("tableGear5th")[0].innerText;
	document.getElementById("g6").value = tableRow.parentElement.parentElement.getElementsByClassName("tableGear6th")[0].innerText;
	document.getElementById("fd").value = tableRow.parentElement.parentElement.getElementsByClassName("tableFinalDrive")[0].innerText;
	
}