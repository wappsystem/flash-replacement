import flash.external.ExternalInterface;
/////Input Parameters//////
var nback:Number;
var numtrials:Number;	//50
//var numpretrials:Number; //3
var letters:Array;
var cuestarttostimulusstarttime:Number;
var interstimulustime:Number;
var cuevisibletime:Number;
var stimulusvisibletime:Number;
var armseq:String;
var vid:String;
var pid:String;
var uid:String;
var rid:String;
var RandomisationNumber:String;
var Initials:String;
var nexttestname:String;
var numpretrials:Number;
///////////////////////////

/////Output Parameters/////
var trialnumbers:Array = Array();
var trialresults:Array = Array();
var trialtimes:Array = Array();
///////////////////////////

//VARIABLES
var outputServer:String;
var NUM_LOCATIONS:Number = 12;	//equal to the number of markers
var NUM_LETTERS:Number;
var trialLocations:Array;
var trialLetters:Array;
var randomLocation:RandomGenerator;
var randomLetter:RandomGenerator;
var startCue_ID:Number;
var startStimulus_ID:Number;
var stopCue_ID:Number;
var stopStimulus_ID:Number;
var delay_ID:Number;
var trialNumber:Number;
var KeyListener:Object = new Object();
var OKListener:Object = new Object();
var startResponseTime:Number;
var responded:Boolean;
//END VARIABLES

/* INIT APP */
Next_btn._visible = false;
saveMessage_txt._visible = false;
serverError_txt._visible = false;
testFinished_txt._visible = false;
Cue_mc._visible = false;
DisplayLetter_mc._visible = false;
loadInput();
/* END INIT */

function loadInput(){
	nback = 3;
	numtrials = 50;
	numpretrials = 3;
	var lettersAsString:String ="B,C,F,H,L,M,P,T,W,X,Y,Z";
	letters = lettersAsString.split(",");
	cuestarttostimulusstarttime = 1500;
	interstimulustime = 4500;
	cuevisibletime = 200;
	stimulusvisibletime = 250;
	//autoClose = _level0.autoClose;
			
	NUM_LETTERS = letters.length;
	randomLocation = new RandomGenerator(NUM_LOCATIONS);
	randomLetter = new RandomGenerator(letters.length);
	var pageNumber:Number;
	pageNumber=nback;
	instruction_txt.html = true;
	var story:XML = new XML();
	story.ignoreWhite = true;
	story.load("modules/common-code/flash/NBack_"+pageNumber.toString()+".htm");
	story.onLoad = function () {	
		instruction_txt.htmlText = story;
	}			
	instruction_txt._visible=true;
	Next_btn._visible = true;
}

Next_btn.onPress = function()
{
	nextTest();
}

function nextTest()
{
	instruction_txt._visible=false;
	Next_btn._visible = false;
	trialNumber = 0;
	randomizeTest();
	startCue();
	startCue_ID = setInterval(startCue,interstimulustime);
	delay_ID = setInterval(cueToStimulusDelay,cuestarttostimulusstarttime);
}

function endTest():Void
{
	clearInterval(startCue_ID);
	clearInterval(startStimulus_ID);
	Key.removeListener(KeyListener);
	/*if (!responded)
	{
		var responseTime:Number = getTimer() - startResponseTime;
		var resultString:String = "O";
		var resultsArr:Array = new Array(trialNumber-nback,resultString,responseTime);
		writeData(resultsArr);
	}*/
	testFinished_txt.text="Please Wait";
	testFinished_txt._visible = true;
	trace(trialnumbers);
	trace(trialresults);
	trace(trialtimes);
	uploadResults();
}

function uploadResults()
	
{	
	//ver 3 call javascript
	var param=	'&NNO='+ trialnumbers.toString()+
				'&NAC='+trialresults.toString()+
		   		'&NRT='+ trialtimes.toString();
	ExternalInterface.call("g_nback3_callback",param);
	testFinished_txt._visible = false;
}

KeyListener.onKeyUp = function()
{
	Key.removeListener(KeyListener);
	
	responded = true;
	var responseTime:Number = getTimer() - startResponseTime;
	var match:Boolean;
	var success:Boolean;
	var wrongKey:Boolean = false;
	var resultString:String;
	
	if (nback == 1)
		match = (trialLocations[trialNumber-1] == trialLocations[0]);
	else
		match = (trialLocations[trialNumber-1] == trialLocations[trialNumber-1-nback]);
	
	//match key pressed
	if ((chr(Key.getAscii())) == "m" || (chr(Key.getAscii())) == "M")
		success = match;
		
	//non-match key pressed
	else if ((chr(Key.getAscii())) == "n" || (chr(Key.getAscii())) == "N")
		success = !match;
		
	//other key pressed
	else
		wrongKey = true;
		
	if (wrongKey)
		resultString = "W";
	else
	{
		if (success)
			resultString = "C";
		else
			resultString = "F";
		
		if (match)
			resultString += "M";
		else
			resultString += "N";
	}
	
	var resultsArr:Array = new Array(trialNumber-nback,resultString,responseTime);	
	writeData(resultsArr);
}

function randomizeTest():Void
{
	var totalNum:Number = nback + numpretrials + numtrials;
	
	trialLocations = new Array(totalNum);
	trialLetters = new Array(totalNum);
	
	for(var i:Number = 0; i < trialLocations.length; i++)
	{
		//starters
		if (i < nback)
		{
			trialLocations[i] = randomLocation.generateRandomNumber(null);
		}
		//warm-up
		else if (i < (nback + numpretrials))
		{
			//50% chance match
			if (Math.random() < 0.5)
			{
				//force match nback = 1
				if (nback == 1)
					trialLocations[i] = trialLocations[0];
				//force match nback > 1
				else
					trialLocations[i] = trialLocations[i-nback];
					
				randomLocation.setLastRandom(trialLocations[i]);
			}
			else
			{
				//force no-match nback = 1
				if (nback == 1)
					trialLocations[i] = randomLocation.generateRandomNumber(trialLocations[0]);
				//force no-match nback > 1
				else
					trialLocations[i] = randomLocation.generateRandomNumber(trialLocations[i-nback]);
			}
		}
		//trials
		else
		{
			//force no-match nback = 1
			if (nback == 1)
				trialLocations[i] = randomLocation.generateRandomNumber(trialLocations[0]);
			//force no-match nback > 1
			else
				trialLocations[i] = randomLocation.generateRandomNumber(trialLocations[i-nback]);
		}
	
		//random letter every time
		trialLetters[i] = randomLetter.generateRandomNumber(null);
	}
	
	//fill in exactly 50% matches.
	var matchFixerGenerator:RandomGenerator = new RandomGenerator(numtrials);
	var matchFixerArray:Array = matchFixerGenerator.generateRandomArray(Math.floor(numtrials/2)).sort(Array.NUMERIC);
	
	for (var i:Number = 0; i < matchFixerArray.length; i++)
	{
		randomLocation.setLastRandom(trialLocations[nback + numpretrials + matchFixerArray[i] - 1]);
			
		if (nback == 1)
		{
			trialLocations[nback + numpretrials + matchFixerArray[i]] = trialLocations[0]; 
		}
		else
		{
			trialLocations[nback + numpretrials + matchFixerArray[i]] = trialLocations[nback + numpretrials + matchFixerArray[i] - nback];
			
			for (var j:Number = nback + numpretrials + matchFixerArray[i] + nback; j < trialLocations.length; j += nback)
			{
				randomLocation.setLastRandom(trialLocations[j-1]);
				trialLocations[j] = randomLocation.generateRandomNumber(trialLocations[j-nback]);
			}
		}
	}
	
}

function writeData(dataArr:Array):Void
{
	var resultStr:String = "";
	for (var i:Number = 0; i < dataArr.length; i++)
	{
		resultStr += dataArr[i] + " ";
	}
	trace(resultStr);
	var testNum:Number = dataArr[0];
	if (testNum <= numpretrials)
	{
		trialnumbers.push("PRE");
	}
	else
	{
		trialnumbers.push(testNum-numpretrials);
	}
	trialresults.push(dataArr[1]);
	trialtimes.push(dataArr[2]);
}

function cueToStimulusDelay():Void
{
	clearInterval(delay_ID);
	startStimulus();
	startStimulus_ID = setInterval(startStimulus,interstimulustime);
}

function startCue(): Void
{
	if (trialNumber < trialLocations.length)
	{
		Cue_mc._visible = true;
		stopCue_ID = setInterval(stopCue,cuevisibletime);
	}
}

function stopCue(): Void
{
	clearInterval(stopCue_ID);
	Cue_mc._visible = false;
}

function startStimulus():Void
{
	if (trialNumber > nback)
	{
		if (!responded)
		{
			var responseTime:Number = getTimer() - startResponseTime;
			var resultString:String = "O";
			var resultsArr:Array = new Array(trialNumber-nback,resultString,responseTime);
			writeData(resultsArr);
		}
	}
	
	if (trialNumber == trialLocations.length)
	{
		endTest();
	}
	else
	{
		changeLetter();
		changeLocation();
		trialNumber++;
		DisplayLetter_mc._visible = true;
		stopStimulus_ID = setInterval(stopStimulus,stimulusvisibletime);
		startResponseTime = getTimer();
		if (trialNumber > nback)
		{
			responded = false;
			Key.addListener(KeyListener);
		}
	}
}

function stopStimulus():Void
{
	clearInterval(stopStimulus_ID);
	DisplayLetter_mc._visible = false;
}

function changeLetter():Void
{
	DisplayLetter_mc.Letter_txt.text = letters[trialLetters[trialNumber]];
}

function changeLocation():Void
{	 
	DisplayLetter_mc._x = eval("marker"+trialLocations[trialNumber]+"_mc._x");
	DisplayLetter_mc._y = eval("marker"+trialLocations[trialNumber]+"_mc._y");
}