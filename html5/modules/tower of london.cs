import flash.external.ExternalInterface;
/////Input Parameters//////
var maxattempts:Number;
var examplequestions:Number;
var startpositions:Array;
var goals:Array;
var moves:Array;
var submoves:String;
var armseq:String;
var vid:String;
var pid:String;
var uid:String;
var rid:String;
var RandomisationNumber:String;
var Initials:String;
var nexttestname:String;

///////////////////////////

/////Output Parameters/////
var exampleplantime:Array = new Array();
var examplefinishtime:Array = new Array();
var examplesuccess:Array = new Array();
var examplenumber:Array = new Array();
var testplantime:Array = new Array();
var testfinishtime:Array = new Array();
var testsuccess:Array = new Array();
var testnumber:Array = new Array();
///////////////////////////

//VARIABLES
var outputServer:String;
var testIndex:Number;
var testAttempts:Number;
var testPassed:Boolean;
var score:Number;
var maxMoves:Number;
var usedMoves:Number;
var allStands:Array = new Array();
var topMostObject:Object;
var firstMoveDone:Boolean;
var startTime:Number;
//END VARIABLES

/* INIT APP */
nextTest_btn._visible = false;
saveMessage_txt._visible = false;
serverError_txt._visible = false;
testFinished_txt._visible = false;
testIndex = 1;
testAttempts = 0;
testPassed = false;
score = 0;
firstMoveDone = false;
generateDemoTower();
generateInteractiveTower();
loadInput();
//fudge();
/* END INIT */

function fudge()
{
	maxattempts = 3;
	examplequestions = 1;
	var startAsString:String = "1g1r2b";
	startpositions = startAsString.split(",");
	var goalsAsString:String = "1g1b3r,2b2g3r,1g2r3b,1g1b2r,1g2b2r,1r1g2b,1b2r2g,1r1b3g,2r2b3g,1r1g1b,1r1b1g,1b1g2r,1b2r3g";
//	var goalsAsString:String = "1g1b3r,2b2g3r";
	goals = goalsAsString.split(",");
	var movesAsString:String = "2,2,2,3,3,4,4,4,4,5,5,5,5";
//	var movesAsString:String = "2,2";
	moves = movesAsString.split(",");
	nextTest_btn._visible = true;
	nextTest();
}

function loadInput()
{
	fudge();
	return;
}

var keyHandler:Object = new Object();
keyHandler.onKeyDown = function()
{
	var selectedBall:TowerBall;
	if (blueBall_mc.isSelected)
	{
		selectedBall = blueBall_mc;
	}
	else if (redBall_mc.isSelected)
	{
		selectedBall = redBall_mc;
	}
	else if (greenBall_mc.isSelected)
	{
		selectedBall = greenBall_mc;
	}
	
	if (Key.getCode() == Key.ESCAPE)
	{
		selectedBall.deSelectBall();
	}
	else
	{
		switch (chr(Key.getAscii()))
		{
			case "r":
				redBall_mc.selectBall(true);
				break;
			case "b":
				blueBall_mc.selectBall(true);
				break;
			case "g":
				greenBall_mc.selectBall(true);
				break;
			case "1":
				selectedBall.moveBall(1);
				break;
			case "2":
				selectedBall.moveBall(2);
				break;
			case "3":
				selectedBall.moveBall(3);
				break;
		}
	}
}
Key.addListener(keyHandler);

function generateDemoTower()
{
	/////////////// Generate DEMO Tower /////////////
	this.attachMovie("TowerStand_mc","demo_stand1_mc",1,{positionsMax:3,myNumber:null,myMarker:null,myBalls:[]});
	with(demo_stand1_mc)
	{
		_x = 500;
		_y = 100;
		_height = 150;
	}
	this.attachMovie("TowerStand_mc","demo_stand2_mc",2,{positionsMax:2,myNumber:null,myMarker:null,myBalls:[]});
	with(demo_stand2_mc)
	{
		_x = 600;
		_y = 150;
		_height = 100;
	}
	this.attachMovie("TowerStand_mc","demo_stand3_mc",3,{positionsMax:1,myNumber:null,myMarker:null,myBalls:[]});
	with(demo_stand3_mc)
	{
		_x = 700;
		_y = 200;
		_height = 50;
	}
	demo_blueBall_mc.interactive = false;
	demo_blueBall_mc.useHandCursor = false;
	demo_redBall_mc.interactive = false;
	demo_redBall_mc.useHandCursor = false;
	demo_greenBall_mc.interactive = false;
	demo_greenBall_mc.useHandCursor = false;
	///////////////////////////////////////////////
}

function generateInteractiveTower()
{
	/////////////// Generate INTERACTIVE Tower //////////////
	this.attachMovie("TowerStand_mc","stand1_mc",11,{positionsMax:3,myNumber:number1_mc,myMarker:marker1_mc,myBalls:[]});
	with(stand1_mc)
	{
		_x = 100;
		_y = 100;
		_height = 150;
		hideNumber();
		hideMarker();
		init();
	}
	allStands.push(stand1_mc);
	this.attachMovie("TowerStand_mc","stand2_mc",12,{positionsMax:2,myNumber:number2_mc,myMarker:marker2_mc,myBalls:[]});
	with(stand2_mc)
	{
		_x = 200;
		_y = 150;
		_height = 100;
		hideNumber();
		hideMarker();
		init();
	}
	allStands.push(stand2_mc);
	this.attachMovie("TowerStand_mc","stand3_mc",13,{positionsMax:1,myNumber:number3_mc,myMarker:marker3_mc,myBalls:[]});
	with(stand3_mc)
	{
		_x = 300;
		_y = 200;
		_height = 50;
		hideNumber();
		hideMarker();
		init();
	}
	allStands.push(stand3_mc);
	blueBall_mc.interactive = true;
	redBall_mc.interactive = true;
	greenBall_mc.interactive = true;
	///////////////////////////////////////////
	
	//// Arrange depths //////////////////////
	//component needed for DepthManager.setDepthAbove to work properly.
	//"Button" component should handle this.
	demo_base_mc.setDepthAbove(demo_stand3_mc);
	base_mc.setDepthAbove(stand3_mc);
	
	demo_blueBall_mc.setDepthAbove(demo_base_mc);
	demo_redBall_mc.setDepthAbove(demo_blueBall_mc);
	demo_greenBall_mc.setDepthAbove(demo_redBall_mc);
	blueBall_mc.setDepthAbove(base_mc);
	redBall_mc.setDepthAbove(blueBall_mc);
	greenBall_mc.setDepthAbove(redBall_mc);
	/////////////////////////////////////////////

	topMostObject = greenBall_mc;
}

// Bring object to front of screen
function bringToFront(obj:Object):Void
{
	obj.setDepthAbove(topMostObject);
	topMostObject = obj;
}

function endTest()
{
	maxMoves_txt.text = "";
	//result_txt.textColor = 0x666666;
	//result_txt.text = "FINISHED";
	result_txt.text = "";
	testFinished_txt._visible = true;
	usedMoves_txt._visible = false;
	lockBalls();
	trace("EXAMPLES");
	trace("    Number: "+examplenumber);
	trace("    Success: "+examplesuccess);
	trace("    Plan Time(ms): "+exampleplantime);
	trace("    Move Time(ms): "+examplefinishtime);
	trace("TESTS");
	trace("    Number: "+testnumber);
	trace("    Success: "+testsuccess);
	trace("    Plan Time(ms): "+testplantime);
	trace("    Move Time(ms): "+testfinishtime);
	uploadResults();
}

function uploadResults()
{	
	//ver 3 call javascript
	var param='&TNO='+testnumber.toString()+
		   '&TSS='+ testsuccess.toString()+
		   '&TPT='+ testplantime.toString()+
		   '&TFT='+ testfinishtime.toString();
	ExternalInterface.call("g_tower_of_london_callback",param);
	testFinished_txt._visible = false;
}

// Load the next Test
function nextTest():Void
{
	var moreTests:Boolean;
	
	testAttempts_txt.text = "";
	usedMoves_txt.text = "";
	result_txt.text = "";
	nextTest_btn._visible = false;
	if (testPassed)
	{
		testIndex++;
	}
	if (testMode())
	{
		example_txt._visible = false;
		instructions_txt._visible = false;
	}
	else
	{
		example_txt.text = "EXAMPLE  "+testIndex+"/"+examplequestions;
	}
	moreTests = (setUpStartPosition(testIndex) && setUpGoal(testIndex));
	if (moreTests)
	{
		if (testMode())
		{
			testnumber.push(testIndex);
		}
		else
		{
			examplenumber.push(testIndex);
		}
	}
	else
	{
		endTest();
	}
}

// Lock the interactive balls from being dragged (when test is finished)
function lockBalls():Void
{
	blueBall_mc.movable = false;
	redBall_mc.movable = false;
	greenBall_mc.movable = false;	
}

// Compare the interactive tower with the demo tower
function checkResult():Void
{	
	var match:Boolean = true;
	
	if (stand1_mc.getBallColours().length != demo_stand1_mc.getBallColours().length)
	{
		match = false;
	}
	else if (stand2_mc.getBallColours().length != demo_stand2_mc.getBallColours().length)
	{
		match = false;
	}
	else if (stand3_mc.getBallColours().length != demo_stand3_mc.getBallColours().length)
	{
		match = false;
	}
	
	for (var i:Number = 0; i < stand1_mc.getBallColours().length; i++)
	{
		if (stand1_mc.getBallColours()[i] != demo_stand1_mc.getBallColours()[i])
		{
			match = false;
			break;
		}
	}
	
	for (var i:Number = 0; i < stand2_mc.getBallColours().length; i++)
	{
		if (stand2_mc.getBallColours()[i] != demo_stand2_mc.getBallColours()[i])
		{
			match = false;
			break;
		}
	}
	
	for (var i:Number = 0; i < stand3_mc.getBallColours().length; i++)
	{
		if (stand3_mc.getBallColours()[i] != demo_stand3_mc.getBallColours()[i])
		{
			match = false;
			break;
		}
	}

	if (!firstMoveDone)
	{
		var timeTaken:Number = getTimer() - startTime;
		if (testMode())
		{
			testplantime.push(timeTaken);
		}
		else
		{
			exampleplantime.push(timeTaken);
		}
		firstMoveDone = true;
		startTime = getTimer();
	}
	
	if (match)
	{
		var timeTaken:Number = getTimer() - startTime;
		if (testMode())
		{
			testfinishtime.push(timeTaken);
			testsuccess.push(true);
		}
		else
		{
			examplefinishtime.push(timeTaken);
			examplesuccess.push(true);
		}
		lockBalls();
		result_txt.textColor = 0x00CC00
		result_txt.text = "PASSED";
		if (testIndex == examplequestions)
		{
			nextTest_btn.label = "Start Test";
		}
		else
		{
			nextTest_btn.label = "Next";
		}
		nextTest_btn._visible = true;
		testPassed = true;
		switch (testAttempts)
		{
			case 0:
				score += 3;
				break;
			case 1:
				score += 2;
				break;
			case 2:
				score += 1;
				break;
		}
	}
	else if (usedMoves == maxMoves)
	{
		var timeTaken:Number = getTimer() - startTime;
		if (testMode())
		{
			testfinishtime.push(timeTaken);
			testsuccess.push(false);
		}
		else
		{
			examplefinishtime.push(timeTaken);
			examplesuccess.push(false);
		}
		lockBalls();
		result_txt.textColor = 0xCC0000
		result_txt.text = "FAILED";
		if (testMode() == false)
		{
			nextTest_btn.label = "Retry";
		}
		else
		{
			nextTest_btn.label = "Next";
		}
		nextTest_btn._visible = true;
		testAttempts++;
		if (testAttempts == maxattempts && testMode())
		{
			testPassed = true;
		}
		else
		{
			testPassed = false;
		}
	} 
	
	if (testPassed)
	{
		testAttempts = 0;
	}
}

nextTest_btn.onPress = function()
{
	nextTest();
}

// Set up goal ball positions on the demo Tower
function setUpGoal(testNum:Number):Boolean
{
	var haveTest:Boolean;
	
	demo_stand1_mc.empty();
	demo_stand2_mc.empty();
	demo_stand3_mc.empty();
	
	if (testNum > goals.length)
	{
		haveTest = false;
	}
	else
	{
		var goalString:String = goals[testNum-1];
		
		var pos1:String = goalString.charAt(0);
		var ball1:String = goalString.charAt(1);
		
		var pos2:String = goalString.charAt(2);
		var ball2:String = goalString.charAt(3);
		
		var pos3:String = goalString.charAt(4);
		var ball3:String = goalString.charAt(5);
		
		var ballX:MovieClip;
		
		switch(ball1)
		{
			case "r": ballX = demo_redBall_mc; break;
			case "g": ballX = demo_greenBall_mc; break;
			case "b": ballX = demo_blueBall_mc; break;
		}
		
		switch (pos1)
		{
			case "1": demo_stand1_mc.addBall(ballX); break;
			case "2": demo_stand2_mc.addBall(ballX); break;
			case "3": demo_stand3_mc.addBall(ballX); break;
		}
		
		switch(ball2)
		{
			case "r": ballX = demo_redBall_mc; break;
			case "g": ballX = demo_greenBall_mc; break;
			case "b": ballX = demo_blueBall_mc; break;
		}
		
		switch (pos2)
		{
			case "1": demo_stand1_mc.addBall(ballX); break;
			case "2": demo_stand2_mc.addBall(ballX); break;
			case "3": demo_stand3_mc.addBall(ballX); break;
		}
		
		switch(ball3)
		{
			case "r": ballX = demo_redBall_mc; break;
			case "g": ballX = demo_greenBall_mc; break;
			case "b": ballX = demo_blueBall_mc; break;
		}
		
		switch (pos3)
		{
			case "1": demo_stand1_mc.addBall(ballX); break;
			case "2": demo_stand2_mc.addBall(ballX); break;
			case "3": demo_stand3_mc.addBall(ballX); break;
		}
		
		maxMoves = moves[testNum-1];
	
		haveTest = true;
	}
	
	if (haveTest)
	{
		maxMoves_txt.text = "Number of moves: "+maxMoves;
	}
	
	return haveTest;
	
	/*
	switch (testNum){
		case 1:
			demo_stand1_mc.addBall(demo_greenBall_mc);
			demo_stand1_mc.addBall(demo_blueBall_mc);
			demo_stand3_mc.addBall(demo_redBall_mc);
			maxMoves = 2;
			haveTest = true;
			break;
		case 2:
			demo_stand2_mc.addBall(demo_blueBall_mc);
			demo_stand2_mc.addBall(demo_greenBall_mc);
			demo_stand3_mc.addBall(demo_redBall_mc);
			maxMoves = 2;
			haveTest = true;
			break;
		case 3:
			demo_stand1_mc.addBall(demo_greenBall_mc);
			demo_stand2_mc.addBall(demo_redBall_mc);
			demo_stand3_mc.addBall(demo_blueBall_mc);
			maxMoves = 2;
			haveTest = true;
			break;
		case 4:
			demo_stand1_mc.addBall(demo_greenBall_mc);
			demo_stand1_mc.addBall(demo_blueBall_mc);
			demo_stand2_mc.addBall(demo_redBall_mc);
			maxMoves = 3;
			haveTest = true;
			break;
		case 5:
			demo_stand1_mc.addBall(demo_greenBall_mc);
			demo_stand2_mc.addBall(demo_blueBall_mc);
			demo_stand2_mc.addBall(demo_redBall_mc);
			maxMoves = 3;
			haveTest = true;
			break;
		case 6:
			demo_stand1_mc.addBall(demo_redBall_mc);
			demo_stand1_mc.addBall(demo_greenBall_mc);
			demo_stand2_mc.addBall(demo_blueBall_mc);
			maxMoves = 4;
			haveTest = true;
			break;
		case 7:
			demo_stand1_mc.addBall(demo_blueBall_mc);
			demo_stand2_mc.addBall(demo_redBall_mc);
			demo_stand2_mc.addBall(demo_greenBall_mc);
			maxMoves = 4;
			haveTest = true;
			break;
		case 8:
			demo_stand1_mc.addBall(demo_redBall_mc);
			demo_stand1_mc.addBall(demo_blueBall_mc);
			demo_stand3_mc.addBall(demo_greenBall_mc);
			maxMoves = 4;
			haveTest = true;
			break;
		case 9:
			demo_stand2_mc.addBall(demo_redBall_mc);
			demo_stand2_mc.addBall(demo_blueBall_mc);
			demo_stand3_mc.addBall(demo_greenBall_mc);
			maxMoves = 4;
			haveTest = true;
			break;
		case 10:
			demo_stand1_mc.addBall(demo_redBall_mc);
			demo_stand1_mc.addBall(demo_greenBall_mc);
			demo_stand1_mc.addBall(demo_blueBall_mc);
			maxMoves = 5;
			haveTest = true;
			break;
		case 11:
			demo_stand1_mc.addBall(demo_redBall_mc);
			demo_stand1_mc.addBall(demo_blueBall_mc);
			demo_stand1_mc.addBall(demo_greenBall_mc);
			maxMoves = 5;
			haveTest = true;
			break;
		case 12:
			demo_stand1_mc.addBall(demo_blueBall_mc);
			demo_stand1_mc.addBall(demo_greenBall_mc);
			demo_stand2_mc.addBall(demo_redBall_mc);
			maxMoves = 5;
			haveTest = true;
			break;
		case 13:
			demo_stand1_mc.addBall(demo_blueBall_mc);
			demo_stand2_mc.addBall(demo_redBall_mc);
			demo_stand3_mc.addBall(demo_greenBall_mc);
			maxMoves = 5;
			haveTest = true;
			break;
		default:
			haveTest = false;;
	}
	*/

}

// Set up the starting ball positions on the interactive Tower
function setUpStartPosition(testNum:Number):Boolean
{
	var haveTest:Boolean;
	
	stand1_mc.empty();
	stand2_mc.empty();
	stand3_mc.empty();
	
	usedMoves = 0;
	firstMoveDone = false;
	startTime = getTimer();
	
	score_txt.text = "SCORE: "+score;
	usedMoves_txt.text = "Used: 0 moves";
	testAttempts_txt.text = "Unsuccessful attempts: "+testAttempts;
	
	
	var startString:String;
	if (startpositions.length == 1)
	{
		startString = startpositions[0];
	}
	else
	{
		startString = startpositions[testNum-1];
	}
		
	var pos1:String = startString.charAt(0);
	var ball1:String = startString.charAt(1);
	
	var pos2:String = startString.charAt(2);
	var ball2:String = startString.charAt(3);
	
	var pos3:String = startString.charAt(4);
	var ball3:String = startString.charAt(5);
	
	var ballX:MovieClip;
	
	switch(ball1)
	{
		case "r": ballX = redBall_mc; break;
		case "g": ballX = greenBall_mc; break;
		case "b": ballX = blueBall_mc; break;
	}
	
	switch (pos1)
	{
		case "1": stand1_mc.addBall(ballX); break;
		case "2": stand2_mc.addBall(ballX); break;
		case "3": stand3_mc.addBall(ballX); break;
	}
	
	switch(ball2)
	{
		case "r": ballX = redBall_mc; break;
		case "g": ballX = greenBall_mc; break;
		case "b": ballX = blueBall_mc; break;
	}
	
	switch (pos2)
	{
		case "1": stand1_mc.addBall(ballX); break;
		case "2": stand2_mc.addBall(ballX); break;
		case "3": stand3_mc.addBall(ballX); break;
	}
	
	switch(ball3)
	{
		case "r": ballX = redBall_mc; break;
		case "g": ballX = greenBall_mc; break;
		case "b": ballX = blueBall_mc; break;
	}
	
	switch (pos3)
	{
		case "1": stand1_mc.addBall(ballX); break;
		case "2": stand2_mc.addBall(ballX); break;
		case "3": stand3_mc.addBall(ballX); break;
	}
	
	haveTest = true;
	
			//stand1_mc.addBall(greenBall_mc);
			//stand1_mc.addBall(redBall_mc);
			//stand2_mc.addBall(blueBall_mc);
			//haveTest = true;

	return haveTest;
}

function testMode():Boolean
{
	if (testIndex > examplequestions)
	{
		return true;
	}
	else
	{
		return false;
	}
}