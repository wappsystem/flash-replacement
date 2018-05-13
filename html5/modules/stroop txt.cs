import flash.external.ExternalInterface;
import flash.display.LoaderInfo;
//2006-12-15
// Input from server is 1 or 0 not true and false for 'checkforcolor'
// so checkforcolor was changed to number.
//
// INPUT
var words:Array;
var colors:Array;
var delaytime:Number;
var timelimit:Number;
var checkforcolor:Number;
var armseq:String;
var vid:String;
var pid:String;
var uid:String;
var rid:String;
var nexttestname:String;
var Initials:String;
var RandomisationNumber:String;

//var autoClose:Boolean;
// END INPUT

// OUTPUT Parameters
var correct:Number = 0;
var ccorr:Number = 0;
var incorrect:Number = 0;
var testreactiontime:Array = new Array();
var testsuccess:Array = new Array();
var presentedtext:Array = new Array();
var presentedcolour:Array = new Array();
var respondedcolour:Array = new Array();

///////////////////////////


// END OUTPUT

// VARIABLES
var outputServer:String;
var responseBoxes:Array = new Array(words.length);
var lastWord:String = "";
var lastColor:Number = -1;
var delaytimeID:Number;
var timelimitID:Number;
var startTime:Number;
var story:XML = new XML();
// END VARIABLES
  
/* INIT APP */
saveMessage_txt._visible = false;
serverError_txt._visible = false;
testFinished_txt._visible = false;
	
loadInput();
/* END INIT */

start_btn.onPress = function()
{
	start_btn._visible = false;
	Instruction_text._visible =false;
	
	

	
	startApp();
}

function loadInput()
{
	trace(loaderInfo)
	delaytime =0;
	timelimit =45000;
	checkforcolor=0;
	var wordsAsString:String = "RED,GREEN,BLUE";
	words = wordsAsString.split(",");
	var colorsAsString:String = "0xFF0000,0x00FF00,0x0000FF";
	colors = colorsAsString.split(",");
	if (checkforcolor > 0)
	{
		content_txt.multiline= true;
		content_txt.wordWrap = true;
		content_txt.html = true;
		var story:XML = new XML();
		story.ignoreWhite = true;
		story.load("modules/common-code/flash/instruction_col.htm");
		story.onLoad = function () {content_txt.htmlText = story;}
	}
	else
	{
		content_txt.multiline= true;
		content_txt.wordWrap = true;
		content_txt.html = true;
		var story:XML = new XML();
		story.ignoreWhite = true;
		story.load("modules/common-code/flash/instruction_txt.htm");
		story.onLoad = function () {content_txt.htmlText = story;}	
	}
}
function startApp()
{
	content_txt._visible =false;
	createResponseBoxes();

	timelimitID = setInterval(timelimitReached,timelimit);
	generateDisplayWord();

	//userClicked takes over from here

	//timelimitReached will end the app
}

function userClicked()
{
	setResponseBoxesVisible(false);
	displayWord_txt._visible = false;

	var responseNameSplit:Array = this._name.split('_');
	var responseColor:String = responseNameSplit[0];
	var timeTaken:Number = getTimer() - startTime;
	respondedcolour.push(responseColor);
	if(checkforcolor > 0 )
	{
		var displayWordColor:String = "";
		var colorI:Number;
		for(colorI = 0; colorI < words.length; colorI++)
		{
			if (colors[colorI] == lastColor)
			{
				displayWordColor = words[colorI];
				break;
			}
		}
						
		if (responseColor == displayWordColor)
		{
			correct++;
			testreactiontime.push(timeTaken);
			testsuccess.push("C");
		}
		else
		{
			incorrect++;
			testsuccess.push("W");
			testreactiontime.push(timeTaken);
		}
	}
	else
	{
		if (responseColor == lastWord)
		{
			correct++;
			testsuccess.push("C");
			testreactiontime.push(timeTaken);
		}
		else
		{
			incorrect++;
			testsuccess.push("W");
			testreactiontime.push(timeTaken);
		}
	}
	ccorr=correct;
	delaytimeID = setInterval(generateDisplayWord,delaytime);
}

function timelimitReached()
{
	clearInterval(delaytimeID);
	clearInterval(timelimitID);
	displayWord_txt._visible = false;
	setResponseBoxesVisible(false);
	
	//if (!autoClose)
	//{
		testFinished_txt._visible = true;	
	//}
	uploadResults();
	//if (autoClose)
	//{
		//getURL("javascript:window.close()");
	//}

}
function uploadResults()
{	
	//ver 3 call javascript
	testFinished_txt.text='Upload Results';
	var param=	
			'&ReactionTimes='+testreactiontime.toString()+
			'&Result='+testsuccess.toString()+
			'&ShownText='+presentedtext.toString()+
			'&ShownColour='+presentedcolour.toString()+
			'&ResponseColour='+respondedcolour.toString();
	ExternalInterface.call("g_stroop_text_callback",param);
	testFinished_txt._visible = false;
}
function generateDisplayWord()
{
	clearInterval(delaytimeID);
		
	var randomWord:String = lastWord;
	var randomColour:Number = lastColor;
	
	while(randomWord == lastWord)
	{
		randomWord = words[Math.floor(Math.random()*words.length)];
	}
	while(randomColour == lastColor)
	{
		randomColour = colors[Math.floor(Math.random()*colors.length)];
	}
	displayWord_txt.text = randomWord;
	presentedtext.push(randomWord);

	displayWord_txt.textColor = randomColour;
	presentedcolour.push(randomColour);

	
	lastWord = randomWord;
	lastColor = randomColour;
	
	displayWord_txt._visible = true;
	setResponseBoxesVisible(true);
	startTime = getTimer();
}

function createResponseBoxes()
{
	var yPosition:Number;
	var xPosition:Number;
	var responseBoxWidth:Number;
	var BOX_SPACE_WIDTH:Number = 10;
		
	var temp_mc:MovieClip = attachMovie("ResponseBox","temp_mc",getNextHighestDepth());
	responseBoxWidth = temp_mc._width;
	temp_mc.unloadMovie();
	
	yPosition = 250;
	var totalResponseWidth:Number = (words.length-1) * (responseBoxWidth + BOX_SPACE_WIDTH);
	xPosition = Stage.width/2 - totalResponseWidth/2;
	
	var colorI:Number;
	for(colorI = 0; colorI < colors.length; colorI++)
	{
		var mcName:String = words[colorI] + "_mc";
		var responseBox:MovieClip = attachMovie("ResponseBox",mcName,getNextHighestDepth(),{_x:xPosition,_y:yPosition});
		color = new Color(responseBox);
		color.setRGB(colors[colorI]);
		
		responseBox.onPress = userClicked;
		responseBoxes[colorI] = responseBox;
		
		xPosition += (responseBoxWidth + BOX_SPACE_WIDTH);
	}
}

function setResponseBoxesVisible(visibility:Boolean)
{
	var respBoxI:Number;
	var respBox_mc:MovieClip;
	for(respBoxI = 0; respBoxI < responseBoxes.length; respBoxI++)
	{
		respBox_mc = responseBoxes[respBoxI];
		respBox_mc._visible = visibility;
	}
}
