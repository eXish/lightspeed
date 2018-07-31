using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using lightspeed;

public static class Extensions
{
    private static System.Random rng = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

public class lightspeedScript : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMAudio Audio;

    //screenAnimation
    public Material lightOn;
    public Material lightOff;
    public Renderer[] topDots;
    public Renderer[] bottomDots;
    private bool screenAnimatorOn = true;
    private int litDots = 1;

    //statusLevels
    public GameObject antimatterBar;
    public GameObject dilithiumBar;
    public GameObject shieldsBar;
    public int antimatterLevel;
    public int dilithiumLevel;
    public int shieldsLevel;
    public TextMesh antimatterText;
    public TextMesh dilithiumText;
    public TextMesh shieldsText;
    public int stardate;
    public int subStardate;
    public TextMesh stardateText;

    //quadrant
    public string symbol;
    public string[] symbolOptions;
    public TextMesh symbolText;
    public Color[] colorOptions;
    public Color symbolColor;
    public string quadrant;
    public string[] quadrantOptions;
    public Material quadrantActive;
    public Material quadrantInactive;
    public Renderer[] quadrantPoint;

    //screens
    public GameObject warpObjects;
    public GameObject destinationObjects;
    public GameObject officerObjects;
    public GameObject encryptObjects;
    public GameObject engageObjects;
    public bool[] displayedScreen;
    private int flashCount = 0;
    private bool submittingAnswer = false;

    //mainButtons
    public KMSelectable warpSpeedAccessButton;
    public KMSelectable destinationAccessButton;
    public KMSelectable officerAccessButton;
    public KMSelectable encryptButton;
    public KMSelectable engageButton;
    public Animator[] animator;

    //warpPage
    public KMSelectable increaseWarp;
    public KMSelectable decreaseWarp;
    public KMSelectable primeWarp;
    public int setWarpLevel = 0;
    public TextMesh setWarpText;
    public Renderer[] dialSegments;
    public Material dialOn;
    public Material dialOff;
    public TextMesh warpDisplayText;

    //destinationPage
    public KMSelectable planetLeft;
    public KMSelectable planetRight;
    public KMSelectable layInCourse;
    public string[] alphaQuadrantPlanets;
    public string[] alphaDilithiumLevels;
    public string[] alphaClassification;
    public string[] betaQuadrantPlanets;
    public string[] betaDilithiumLevels;
    public string[] betaClassification;
    public string[] gammaQuadrantPlanets;
    public string[] gammaDilithiumLevels;
    public string[] gammaClassification;
    public string[] deltaQuadrantPlanets;
    public string[] deltaDilithiumLevels;
    public string[] deltaClassification;
    public List<String> selectedPlanets = new List<string>();
    public List<String> shuffledPlanets = new List<string>();
    public List<int> selectedDilithiumLevels = new List<int>();
    public List<String> selectedClassification = new List<string>();
    public Texture[] planetImages;
    public Renderer planetImagesDisplay;
    public TextMesh displayedPlanetName;
    private int displayIndex = 0;
    private int planetIndex = 0;
    public List<int> alphaIndexed = new List<int>();
    public List<int> betaIndexed = new List<int>();
    public List<int> gammaIndexed = new List<int>();
    public List<int> deltaIndexed = new List<int>();
    public TextMesh destinationDisplayText;

    //crewPage
    public KMSelectable crewLeft;
    public KMSelectable crewRight;
    public KMSelectable assignCrew;
    public string[] crewmanNames;
    public string[] crewmanDates;
    public string[] ensignNames;
    public string[] ensignDates;
    public string[] lieutenantNames;
    public string[] lieutenantDates;
    public string[] lieutenantCommanderNames;
    public string[] lieutenantCommanderDates;
    public string[] commanderNames;
    public string[] commanderDates;
    public string[] captainNames;
    public string[] captainDates;
    public string[] rankNames;
    public List<String> selectedCrew = new List<string>();
    public List<String> selectedDates = new List<string>();
    public List<int> crewmanIndexed = new List<int>();
    public List<int> ensignIndexed = new List<int>();
    public TextMesh rankText;
    public TextMesh crewNameText;
    private int crewStartIndex = 0;
    public TextMesh crewDisplayText;
    public string rankAbv = "";

    //encryptPage
    public KMSelectable[] keypad;
    public KMSelectable encryptSubmitButton;
    public string enteredEncryptionCode;
    public TextMesh enteredEncryptionCodeDisplay;
    public string digit4 = "";
    public bool dataEncrypted = false;
    public TextMesh encryptDataText;
    public Material fontMat;
    public Font[] fonts;

    //engagePage
    public KMSelectable engageSubmitButton;
    public TextMesh engageSubmitButtonText;
    public KMSelectable destructButton;
    public TextMesh destructButtonText;
    public Renderer[] countdownBars;
    public Material[] countdownMat;
    private bool destructActive;
    private bool destructionOperation;
	private bool destructByTwitch;
	private bool shipDestroyed;
    private bool engageActive;
    private bool engageOperation;
    private bool moduleSolved;

    //answers
    public int correctWarpSpeed = 0;
    public string correctPlanet;
    public string correctClassification;
    public string correctCrew;
    public string submittedOfficer;
    public string correctRank;
    public string correctEncryption = "";

    //SFX
    int soundIndex = 0;
    public AudioClip[] beep;
    public AudioClip[] error;

    //logging
    static int moduleIdCounter = 1;
    int moduleId;


    void Awake()
    {
        moduleId = moduleIdCounter++;
        warpSpeedAccessButton.OnInteract += delegate () { onWarpSpeedAccess(); return false; };
        destinationAccessButton.OnInteract += delegate () { onDestinationAccessButton(); return false; };
        officerAccessButton.OnInteract += delegate () { onOfficerAccessButton(); return false; };
        encryptButton.OnInteract += delegate () { onEncryptButton(); return false; };
        engageButton.OnInteract += delegate () { onEngageButton(); return false; };
        increaseWarp.OnInteract += delegate () { onIncreaseWarp(); return false; };
        decreaseWarp.OnInteract += delegate () { onDecreaseWarp(); return false; };
        primeWarp.OnInteract += delegate () { onPrimeWarp(); return false; };
        planetLeft.OnInteract += delegate () { onPlanetLeft(); return false; };
        planetRight.OnInteract += delegate () { onPlanetRight(); return false; };
        layInCourse.OnInteract += delegate () { onLayInCourse(); return false; };
        crewLeft.OnInteract += delegate () { onCrewLeft(); return false; };
        crewRight.OnInteract += delegate () { onCrewRight(); return false; };
        assignCrew.OnInteract += delegate () { onAssignCrew(); return false; };
        encryptSubmitButton.OnInteract += delegate () { onEncryptSubmitButton(); return false; };
        foreach (KMSelectable number in keypad)
        {
            KMSelectable pressedNumber = number;
            number.OnInteract += delegate () { keypadPress(pressedNumber); return false; };
        }
        engageSubmitButton.OnInteract += delegate () { onEngageSubmitButton(); return false; };
        destructButton.OnInteract += delegate () { onDestructButton(); return false; };
	    Bomb.OnBombExploded += delegate() { shipDestroyed = true; };
    }

    void Start()
    {
        switchOffScreenBools();
        StartCoroutine(screenAnim());
        setWarpLevel = UnityEngine.Random.Range(1,10);
        setWarpText.text = setWarpLevel.ToString();
        warpDialColours();
        warpDisplayText.text = "";
        destinationDisplayText.text = "";
        crewDisplayText.text = "";
        enteredEncryptionCodeDisplay.text = "";
        setStatusLevels();
        setQuadrant();
        calculateWarp();
        setUpPlanetOptions();
        calculatePlanet();
        setUpCrewOptions();
        calculateCrew();
        calculateEncryption();
        int firstDistance = UnityEngine.Random.Range(4,18);
        int secondDistance = UnityEngine.Random.Range(0,10);
        Debug.LogFormat("[Lightspeed #{0}] Captain's Log, stardate {1}.{2}: We are continuing our journey through the {3} quadrant. Antimatter levels are steady at {4}% and dilithium at {5}%. Following our encounter with the Kazon, our shields have fallen to {6}%, and morale is lacking among the crew. If we don't find a habitable world soon, we could be in a lot of trouble.", moduleId, stardate, subStardate, quadrant, antimatterLevel, dilithiumLevel, shieldsLevel);
        Debug.LogFormat("[Lightspeed #{0}] Captain's Log, supplemental: we've calculated that our most efficient warp speed will be warp {1}. Long-range sensors have detected an {2}-class planet called {3} around {4}.{5} light years from our current location. {6} {7} has been assigned to lead an away team to explore the surface.", moduleId, correctWarpSpeed, correctClassification, correctPlanet, firstDistance, secondDistance, correctRank, correctCrew);
        Debug.LogFormat("[Lightspeed #{0}] Correct Warp Speed: Warp {1}.", moduleId, correctWarpSpeed);
        Debug.LogFormat("[Lightspeed #{0}] Correct Planet: {1} ({2}-class).", moduleId, correctPlanet, correctClassification);
        Debug.LogFormat("[Lightspeed #{0}] Correct Ranking Officer: {1} ({2}).", moduleId, correctCrew, correctRank);
        Debug.LogFormat("[Lightspeed #{0}] Correct Encryption Code: {1}.", moduleId, correctEncryption);
    }

    void onWarpSpeedAccess()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if (displayedScreen[0] == false)
        {
            switchOffScreenBools();
            displayedScreen[0] = true;
            warpObjects.SetActive(true);
            animator[1].SetBool("buttonOn", false);
            animator[1].SetBool("buttonOff", true);
            animator[2].SetBool("buttonOn", false);
            animator[2].SetBool("buttonOff", true);
            animator[3].SetBool("buttonOn", false);
            animator[3].SetBool("buttonOff", true);
            animator[4].SetBool("buttonOn", false);
            animator[4].SetBool("buttonOff", true);
            animator[0].SetBool("buttonOn", true);
            animator[0].SetBool("buttonOff", false);
        }
        else
        {
            animator[0].SetBool("buttonOn", false);
            animator[0].SetBool("buttonOff", true);
            switchOffScreenBools();
        }
    }

    void onIncreaseWarp()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.25f);
        Audio.PlaySoundAtTransform(beep[0].name, transform);
        if(moduleSolved)
        {
            return;
        }
        if(setWarpLevel < 9)
        {
            setWarpLevel++;
            setWarpText.text = setWarpLevel.ToString();
            warpDialColours();
        }
    }

    void onDecreaseWarp()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.25f);
        Audio.PlaySoundAtTransform(beep[0].name, transform);
        if(moduleSolved)
        {
            return;
        }
        if(setWarpLevel > 1)
        {
            setWarpLevel -= 1;
            setWarpText.text = setWarpLevel.ToString();
            warpDialColours();
        }
    }

    void onPrimeWarp()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if(moduleSolved)
        {
            return;
        }

        if(dataEncrypted)
        {
            GetComponent<KMBombModule>().HandleStrike();
            Debug.LogFormat("[Lightspeed #{0}] Warning: Data is encrypted. Unable to modify. Decrypt data before continuing.", moduleId);
            soundIndex = UnityEngine.Random.Range(0,2);
            Audio.PlaySoundAtTransform(error[soundIndex].name, transform);
        }
        else
        {
            Audio.PlaySoundAtTransform("inputReceived", transform);
            StartCoroutine(warpToScreen());
        }
    }

    IEnumerator warpToScreen()
    {
        if(submittingAnswer)
        {
            yield return null;
        }
        else
        {
            submittingAnswer = true;
            flashCount = 0;
            string submittedAnswer = setWarpLevel.ToString();
            while(flashCount < 20)
            {
                yield return new WaitForSeconds(0.02f);
                warpDisplayText.text = submittedAnswer;
                yield return new WaitForSeconds(0.02f);
                warpDisplayText.text = "";
                flashCount++;
            }
            warpDisplayText.text = submittedAnswer;
            flashCount = 0;
            submittingAnswer = false;
        }
    }

    void warpDialColours()
    {
        foreach(Renderer segment in dialSegments)
        {
            segment.material = dialOff;
        }
        for(int i = 1; i <=9; i++)
        {
            if(setWarpLevel >= i)
            {
                dialSegments[i-1].material = dialOn;
            }
        }
    }

    void calculateWarp()
    {
        if(quadrant != "Alpha")
        {
            correctWarpSpeed = antimatterLevel / 10;
        }
        else
        {
            correctWarpSpeed = (antimatterLevel / 10) - 2;
        }
        if(quadrant != "Delta")
        {
            if(shieldsLevel > 50 && shieldsLevel <= 75)
            {
                correctWarpSpeed -= 1;
            }
            else if(shieldsLevel > 25 && shieldsLevel <= 50)
            {
                correctWarpSpeed -= 2;
            }
            else if(shieldsLevel < 26)
            {
                correctWarpSpeed -= 3;
            }
        }
        else
        {
            if(shieldsLevel > 70 && shieldsLevel <= 85)
            {
                correctWarpSpeed -= 1;
            }
            else if(shieldsLevel > 55 && shieldsLevel <= 70)
            {
                correctWarpSpeed -= 2;
            }
            else if(shieldsLevel > 40 && shieldsLevel <= 55)
            {
                correctWarpSpeed -= 3;
            }
            else if(shieldsLevel > 25 && shieldsLevel <= 40)
            {
                correctWarpSpeed -= 4;
            }
            else if(shieldsLevel > 10 && shieldsLevel <= 25)
            {
                correctWarpSpeed -= 5;
            }
            else if(shieldsLevel < 10)
            {
                correctWarpSpeed -= 6;
            }
        }
        if(correctWarpSpeed < 1)
        {
            correctWarpSpeed = 1;
        }
    }


    void onDestinationAccessButton()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if (displayedScreen[1] == false)
        {
            switchOffScreenBools();
            displayedScreen[1] = true;
            destinationObjects.SetActive(true);
            animator[0].SetBool("buttonOn", false);
            animator[0].SetBool("buttonOff", true);
            animator[2].SetBool("buttonOn", false);
            animator[2].SetBool("buttonOff", true);
            animator[3].SetBool("buttonOn", false);
            animator[3].SetBool("buttonOff", true);
            animator[4].SetBool("buttonOn", false);
            animator[4].SetBool("buttonOff", true);
            animator[1].SetBool("buttonOn", true);
            animator[1].SetBool("buttonOff", false);
        }
        else
        {
            animator[1].SetBool("buttonOn", false);
            animator[1].SetBool("buttonOff", true);
            switchOffScreenBools();
        }
    }

    void onPlanetLeft()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.25f);
        Audio.PlaySoundAtTransform(beep[0].name, transform);
        if(moduleSolved)
        {
            return;
        }
        planetIndex += 11;
        displayIndex += 11;
        planetIndex %= 12;
        displayIndex %= 12;
        displayedPlanetName.text = shuffledPlanets[planetIndex];
        planetImagesDisplay.material.mainTexture = planetImages[displayIndex];

    }

    void onPlanetRight()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.25f);
        Audio.PlaySoundAtTransform(beep[0].name, transform);
        if(moduleSolved)
        {
            return;
        }
        planetIndex++;
        displayIndex++;
        planetIndex %= 12;
        displayIndex %= 12;
        displayedPlanetName.text = shuffledPlanets[planetIndex];
        planetImagesDisplay.material.mainTexture = planetImages[displayIndex];
    }

    void onLayInCourse()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if(moduleSolved)
        {
            return;
        }
        if(dataEncrypted)
        {
            GetComponent<KMBombModule>().HandleStrike();
            Debug.LogFormat("[Lightspeed #{0}] Warning: Data is encrypted. Unable to modify. Decrypt data before continuing.", moduleId);
            soundIndex = UnityEngine.Random.Range(0,2);
            Audio.PlaySoundAtTransform(error[soundIndex].name, transform);
        }
        else
        {
            Audio.PlaySoundAtTransform("inputReceived", transform);
            StartCoroutine(courseToScreen());
        }
    }

    IEnumerator courseToScreen()
    {
        if(submittingAnswer)
        {
            yield return null;
        }
        else
        {
            submittingAnswer = true;
            flashCount = 0;
            string submittedAnswer = displayedPlanetName.text;
            while(flashCount < 20)
            {
                yield return new WaitForSeconds(0.02f);
                destinationDisplayText.text = submittedAnswer;
                yield return new WaitForSeconds(0.02f);
                destinationDisplayText.text = "";
                flashCount++;
            }
            destinationDisplayText.text = submittedAnswer;
            flashCount = 0;
            submittingAnswer = false;
        }
    }

    void setUpPlanetOptions()
    {
        if(quadrant == "Alpha")
        {
            alphaBasePlanets();
            selectAdditionalAlphaPlanets();
            betaBasePlanets();
            gammaBasePlanets();
            deltaBasePlanets();
        }
        else if (quadrant == "Beta")
        {
            betaBasePlanets();
            selectAdditionalBetaPlanets();
            alphaBasePlanets();
            gammaBasePlanets();
            deltaBasePlanets();
        }
        else if (quadrant == "Gamma")
        {
            gammaBasePlanets();
            selectAdditionalGammaPlanets();
            alphaBasePlanets();
            betaBasePlanets();
            deltaBasePlanets();
        }
        else
        {
            deltaBasePlanets();
            selectAdditionalDeltaPlanets();
            alphaBasePlanets();
            betaBasePlanets();
            gammaBasePlanets();
        }
        displayIndex = UnityEngine.Random.Range(0,12);
        planetImagesDisplay.material.mainTexture = planetImages[displayIndex];
        shuffledPlanets.AddRange(selectedPlanets);
        shuffledPlanets.Shuffle();
        planetIndex = UnityEngine.Random.Range(0,12);
        displayedPlanetName.text = shuffledPlanets[planetIndex];
    }

    void alphaBasePlanets()
    {
        selectedPlanets.Add(alphaQuadrantPlanets[8]);
        selectedDilithiumLevels.Add(int.Parse(alphaDilithiumLevels[8]));
        selectedClassification.Add(alphaClassification[8]);
    }

    void betaBasePlanets()
    {
        selectedPlanets.Add(betaQuadrantPlanets[8]);
        selectedDilithiumLevels.Add(int.Parse(betaDilithiumLevels[8]));
        selectedClassification.Add(betaClassification[8]);
    }

    void gammaBasePlanets()
    {
        selectedPlanets.Add(gammaQuadrantPlanets[8]);
        selectedDilithiumLevels.Add(int.Parse(gammaDilithiumLevels[8]));
        selectedClassification.Add(gammaClassification[8]);
    }

    void deltaBasePlanets()
    {
        selectedPlanets.Add(deltaQuadrantPlanets[8]);
        selectedDilithiumLevels.Add(int.Parse(deltaDilithiumLevels[8]));
        selectedClassification.Add(deltaClassification[8]);
    }

    void selectAdditionalAlphaPlanets()
    {
        if(selectedPlanets.Count() > 8)
        {
            return;
        }
        else
        {
            int numberOfSelectedA = 0;
            while(numberOfSelectedA < 2)
            {
                int alphaIndex = UnityEngine.Random.Range(0,8);
                while(alphaIndexed.Contains(alphaIndex))
                {
                    alphaIndex = UnityEngine.Random.Range(0,8);
                }
                alphaIndexed.Add(alphaIndex);
                selectedPlanets.Add(alphaQuadrantPlanets[alphaIndex]);
                selectedDilithiumLevels.Add(int.Parse(alphaDilithiumLevels[alphaIndex]));
                selectedClassification.Add(alphaClassification[alphaIndex]);
                numberOfSelectedA++;
            }
            selectAdditionalBetaPlanets();
        }
    }

    void selectAdditionalBetaPlanets()
    {
        if(selectedPlanets.Count() > 8)
        {
            return;
        }
        else
        {
            int numberOfSelectedB = 0;
            while(numberOfSelectedB < 2)
            {
                int betaIndex = UnityEngine.Random.Range(0,8);
                while(betaIndexed.Contains(betaIndex))
                {
                    betaIndex = UnityEngine.Random.Range(0,8);
                }
                betaIndexed.Add(betaIndex);
                selectedPlanets.Add(betaQuadrantPlanets[betaIndex]);
                selectedDilithiumLevels.Add(int.Parse(betaDilithiumLevels[betaIndex]));
                selectedClassification.Add(betaClassification[betaIndex]);
                numberOfSelectedB++;
            }
            selectAdditionalGammaPlanets();
        }
    }

    void selectAdditionalGammaPlanets()
    {
        if(selectedPlanets.Count() > 8)
        {
            return;
        }
        else
        {
            int numberOfSelectedC = 0;
            while(numberOfSelectedC < 2)
            {
                int gammaIndex = UnityEngine.Random.Range(0,8);
                while(gammaIndexed.Contains(gammaIndex))
                {
                    gammaIndex = UnityEngine.Random.Range(0,8);
                }
                gammaIndexed.Add(gammaIndex);
                selectedPlanets.Add(gammaQuadrantPlanets[gammaIndex]);
                selectedDilithiumLevels.Add(int.Parse(gammaDilithiumLevels[gammaIndex]));
                selectedClassification.Add(gammaClassification[gammaIndex]);
                numberOfSelectedC++;
            }
            selectAdditionalDeltaPlanets();
        }
    }

    void selectAdditionalDeltaPlanets()
    {
        if(selectedPlanets.Count() > 8)
        {
            return;
        }
        else
        {
            int numberOfSelectedD = 0;
            while(numberOfSelectedD < 2)
            {
                int deltaIndex = UnityEngine.Random.Range(0,8);
                while(deltaIndexed.Contains(deltaIndex))
                {
                    deltaIndex = UnityEngine.Random.Range(0,8);
                }
                deltaIndexed.Add(deltaIndex);
                selectedPlanets.Add(deltaQuadrantPlanets[deltaIndex]);
                selectedDilithiumLevels.Add(int.Parse(deltaDilithiumLevels[deltaIndex]));
                selectedClassification.Add(deltaClassification[deltaIndex]);
                numberOfSelectedD++;
            }
            selectAdditionalAlphaPlanets();
        }
    }

    void calculatePlanet()
    {
        if(selectedDilithiumLevels[2] >= selectedDilithiumLevels[1])
        {
            if (dilithiumLevel >= selectedDilithiumLevels[2])
            {
                correctPlanet = selectedPlanets[2];
                correctClassification = selectedClassification[2];
            }
            else if (dilithiumLevel >= selectedDilithiumLevels[1])
            {
                correctPlanet = selectedPlanets[1];
                correctClassification = selectedClassification[1];
            }
            else
            {
                correctPlanet = selectedPlanets[0];
                correctClassification = selectedClassification[0];
            }
        }
        else
        {
            if (dilithiumLevel >= selectedDilithiumLevels[1])
            {
                correctPlanet = selectedPlanets[1];
                correctClassification = selectedClassification[1];
            }
            else if (dilithiumLevel >= selectedDilithiumLevels[2])
            {
                correctPlanet = selectedPlanets[2];
                correctClassification = selectedClassification[2];
            }
            else
            {
                correctPlanet = selectedPlanets[0];
                correctClassification = selectedClassification[0];
            }
        }
    }

    void onOfficerAccessButton()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if (displayedScreen[2] == false)
        {
            switchOffScreenBools();
            displayedScreen[2] = true;
            officerObjects.SetActive(true);
            animator[0].SetBool("buttonOn", false);
            animator[0].SetBool("buttonOff", true);
            animator[1].SetBool("buttonOn", false);
            animator[1].SetBool("buttonOff", true);
            animator[3].SetBool("buttonOn", false);
            animator[3].SetBool("buttonOff", true);
            animator[4].SetBool("buttonOn", false);
            animator[4].SetBool("buttonOff", true);
            animator[2].SetBool("buttonOn", true);
            animator[2].SetBool("buttonOff", false);
        }
        else
        {
            animator[2].SetBool("buttonOn", false);
            animator[2].SetBool("buttonOff", true);
            switchOffScreenBools();
        }
    }

    void onCrewLeft()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.25f);
        Audio.PlaySoundAtTransform(beep[0].name, transform);
        if(moduleSolved)
        {
            return;
        }
        crewStartIndex += 7;
        crewStartIndex %= 8;
        crewNameText.text = selectedCrew[crewStartIndex];
        rankText.text = rankNames[crewStartIndex];
    }

    void onCrewRight()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.25f);
        Audio.PlaySoundAtTransform(beep[0].name, transform);
        if(moduleSolved)
        {
            return;
        }
        crewStartIndex++;
        crewStartIndex %= 8;
        crewNameText.text = selectedCrew[crewStartIndex];
        rankText.text = rankNames[crewStartIndex];
    }

    void onAssignCrew()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if(moduleSolved)
        {
            return;
        }
        if(dataEncrypted)
        {
            GetComponent<KMBombModule>().HandleStrike();
            Debug.LogFormat("[Lightspeed #{0}] Warning: Data is encrypted. Unable to modify. Decrypt data before continuing.", moduleId);
            soundIndex = UnityEngine.Random.Range(0,2);
            Audio.PlaySoundAtTransform(error[soundIndex].name, transform);
        }
        else
        {
            if(rankText.text == "Crewman")
            {
                rankAbv = "(Cr.)";
            }
            else if(rankText.text == "Ensign")
            {
                rankAbv = "(En.)";
            }
            else if(rankText.text == "Lieutenant")
            {
                rankAbv = "(Lt.)";
            }
            else if(rankText.text == "Lieutenant Commander")
            {
                rankAbv = "(Lt Cm.)";
            }
            else if(rankText.text == "Commander")
            {
                rankAbv = "(Cm.)";
            }
            else
            {
                rankAbv = "(Cpt.)";
            }
            Audio.PlaySoundAtTransform("inputReceived", transform);
            StartCoroutine(crewToScreen());
        }
    }

    IEnumerator crewToScreen()
    {
        if(submittingAnswer)
        {
            yield return null;
        }
        else
        {
            submittingAnswer = true;
            flashCount = 0;
            string submittedAnswer = crewNameText.text + " " + rankAbv;
            submittedOfficer = crewNameText.text;
            while(flashCount < 20)
            {
                yield return new WaitForSeconds(0.02f);
                crewDisplayText.text = submittedAnswer;
                yield return new WaitForSeconds(0.02f);
                crewDisplayText.text = "";
                flashCount++;
            }
            crewDisplayText.text = submittedAnswer;
            flashCount = 0;
            submittingAnswer = false;
        }
    }

    void setUpCrewOptions()
    {
        while(crewmanIndexed.Count < 2)
        {
            int crewIndex = UnityEngine.Random.Range(0,5);
            while(crewmanIndexed.Contains(crewIndex))
            {
                crewIndex = UnityEngine.Random.Range(0,5);
            }
            crewmanIndexed.Add(crewIndex);
            selectedCrew.Add(crewmanNames[crewIndex]);
            selectedDates.Add(crewmanDates[crewIndex]);
        }
        while(ensignIndexed.Count < 2)
        {
            int ensignIndex = UnityEngine.Random.Range(0,5);
            while(ensignIndexed.Contains(ensignIndex))
            {
                ensignIndex = UnityEngine.Random.Range(0,5);
            }
            ensignIndexed.Add(ensignIndex);
            selectedCrew.Add(ensignNames[ensignIndex]);
            selectedDates.Add(ensignDates[ensignIndex]);
        }
        int lieutenantIndex = UnityEngine.Random.Range(0,5);
        selectedCrew.Add(lieutenantNames[lieutenantIndex]);
        selectedDates.Add(lieutenantDates[lieutenantIndex]);
        int lieutenantCommanderIndex = UnityEngine.Random.Range(0,5);
        selectedCrew.Add(lieutenantCommanderNames[lieutenantCommanderIndex]);
        selectedDates.Add(lieutenantCommanderDates[lieutenantCommanderIndex]);
        int commanderIndex = UnityEngine.Random.Range(0,3);
        selectedCrew.Add(commanderNames[commanderIndex]);
        selectedDates.Add(commanderDates[commanderIndex]);
        int captainIndex = UnityEngine.Random.Range(0,2);
        selectedCrew.Add(captainNames[captainIndex]);
        selectedDates.Add(captainDates[captainIndex]);
        crewStartIndex = UnityEngine.Random.Range(0,8);
        rankText.text = rankNames[crewStartIndex];
        crewNameText.text = selectedCrew[crewStartIndex];
    }

    void calculateCrew()
    {
        if(selectedDates[0].Contains(subStardate.ToString()) && correctClassification != "L" && correctClassification != "H" && correctClassification != "Y")
        {
            correctCrew = selectedCrew[0];
            correctRank = rankNames[0];
        }
        else if(selectedDates[1].Contains(subStardate.ToString()) && correctClassification != "L" && correctClassification != "H" && correctClassification != "Y")
        {
            correctCrew = selectedCrew[1];
            correctRank = rankNames[1];
        }
        else if(selectedDates[2].Contains(subStardate.ToString()) && correctClassification != "H" && correctClassification != "Y")
        {
            correctCrew = selectedCrew[2];
            correctRank = rankNames[2];
        }
        else if(selectedDates[3].Contains(subStardate.ToString()) && correctClassification != "H" && correctClassification != "Y")
        {
            correctCrew = selectedCrew[3];
            correctRank = rankNames[3];
        }
        else if(selectedDates[4].Contains(subStardate.ToString()) && correctClassification != "Y")
        {
            correctCrew = selectedCrew[4];
            correctRank = rankNames[4];
        }
        else if(selectedDates[5].Contains(subStardate.ToString()))
        {
            correctCrew = selectedCrew[5];
            correctRank = rankNames[5];
        }
        else if(selectedDates[6].Contains(subStardate.ToString()))
        {
            correctCrew = selectedCrew[6];
            correctRank = rankNames[6];
        }
        else
        {
            correctCrew = selectedCrew[7];
            correctRank = rankNames[7];
        }
    }


    void onEncryptButton()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if (displayedScreen[3] == false)
        {
            switchOffScreenBools();
            displayedScreen[3] = true;
            encryptObjects.SetActive(true);
            animator[0].SetBool("buttonOn", false);
            animator[0].SetBool("buttonOff", true);
            animator[1].SetBool("buttonOn", false);
            animator[1].SetBool("buttonOff", true);
            animator[2].SetBool("buttonOn", false);
            animator[2].SetBool("buttonOff", true);
            animator[4].SetBool("buttonOn", false);
            animator[4].SetBool("buttonOff", true);
            animator[3].SetBool("buttonOn", true);
            animator[3].SetBool("buttonOff", false);
        }
        else
        {
            animator[3].SetBool("buttonOn", false);
            animator[3].SetBool("buttonOff", true);
            switchOffScreenBools();
        }
    }

    void keypadPress(KMSelectable number)
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.25f);
        Audio.PlaySoundAtTransform(beep[0].name, transform);
        if(moduleSolved)
        {
            return;
        }
        if(enteredEncryptionCode.Length > 3)
        {
            enteredEncryptionCode = "";
        }
        if(number == keypad[0])
        {
            enteredEncryptionCode += "0";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
        else if(number == keypad[1])
        {
            enteredEncryptionCode += "1";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
        else if(number == keypad[2])
        {
            enteredEncryptionCode += "2";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
        else if(number == keypad[3])
        {
            enteredEncryptionCode += "3";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
        else if(number == keypad[4])
        {
            enteredEncryptionCode += "4";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
        else if(number == keypad[5])
        {
            enteredEncryptionCode += "5";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
        else if(number == keypad[6])
        {
            enteredEncryptionCode += "6";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
        else if(number == keypad[7])
        {
            enteredEncryptionCode += "7";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
        else if(number == keypad[8])
        {
            enteredEncryptionCode += "8";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
        else if(number == keypad[9])
        {
            enteredEncryptionCode += "9";
            enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
        }
    }

    void onEncryptSubmitButton()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if(moduleSolved)
        {
            return;
        }
        else if(warpDisplayText.text == "" || destinationDisplayText.text == "" || crewDisplayText.text == "")
        {
            GetComponent<KMBombModule>().HandleStrike();
            Debug.LogFormat("[Lightspeed #{0}] Warning: Data entry missing. Unable to encrypt.", moduleId);
            Audio.PlaySoundAtTransform("encryptMissing", transform);
        }
        else
        {
            if(enteredEncryptionCode != correctEncryption)
            {
                GetComponent<KMBombModule>().HandleStrike();
                Debug.LogFormat("[Lightspeed #{0}] Warning: Incorrect encryption code entered.", moduleId);
                Audio.PlaySoundAtTransform("encryptFail", transform);
            }
            else
            {
                if(dataEncrypted)
                {
                    encryptDataText.text = "Encrypt\nData";
                    dataEncrypted = false;
                    Debug.LogFormat("[Lightspeed #{0}] Data decrypted.", moduleId);
                    warpDisplayText.GetComponent<MeshRenderer>().material = fontMat;
                    warpDisplayText.font = fonts[0];
                    destinationDisplayText.GetComponent<MeshRenderer>().material = fontMat;
                    destinationDisplayText.font = fonts[0];
                    crewDisplayText.GetComponent<MeshRenderer>().material = fontMat;
                    crewDisplayText.font = fonts[0];
                }
                else
                {
                    encryptDataText.text = "Decrypt\nData";
                    dataEncrypted = true;
                    Debug.LogFormat("[Lightspeed #{0}] Data encrypted.", moduleId);
                    warpDisplayText.GetComponent<MeshRenderer>().material = fontMat;
                    warpDisplayText.font = fonts[1];
                    destinationDisplayText.GetComponent<MeshRenderer>().material = fontMat;
                    destinationDisplayText.font = fonts[1];
                    crewDisplayText.GetComponent<MeshRenderer>().material = fontMat;
                    crewDisplayText.font = fonts[1];
                }
                Audio.PlaySoundAtTransform("encryptSuccess", transform);
                StartCoroutine(encryptionToScreen());
            }
        }
        enteredEncryptionCode = "";
        enteredEncryptionCodeDisplay.text = enteredEncryptionCode;
    }

    IEnumerator encryptionToScreen()
    {
      {
          if(submittingAnswer)
          {
              yield return null;
          }
          else
          {
              string warpAnswer = warpDisplayText.text;
              string destAnswer = destinationDisplayText.text;
              string crewAnswer = crewDisplayText.text;
              submittingAnswer = true;
              flashCount = 0;
              while(flashCount < 20)
              {
                  yield return new WaitForSeconds(0.02f);
                  warpDisplayText.text = warpAnswer;
                  destinationDisplayText.text = destAnswer;
                  crewDisplayText.text = crewAnswer;
                  yield return new WaitForSeconds(0.02f);
                  warpDisplayText.text = "";
                  destinationDisplayText.text = "";
                  crewDisplayText.text = "";
                  flashCount++;
              }
              warpDisplayText.text = warpAnswer;
              destinationDisplayText.text = destAnswer;
              crewDisplayText.text = crewAnswer;
              flashCount = 0;
              submittingAnswer = false;
          }
      }
    }

    void calculateEncryption()
    {
        string digit1 = (stardate / 10000).ToString();
        int sd2 = ((stardate / 1000) % 10);
        int sd3 = ((stardate / 100) % 10);
        int sd4 = ((stardate / 10) % 10);
        int sd5 = (stardate % 10);
        string digit2 = ((sd2+sd3+sd4+sd5) % 10).ToString();
        string digit3 = ((subStardate + Bomb.GetPortPlates().Count() + Bomb.GetOnIndicators().Count()) % 10).ToString();

        if(Bomb.GetBatteryCount() <= 3)
        {
            if(correctRank == "Commander" || correctRank == "Captain")
            {
                if(quadrant == "Alpha" || quadrant == "Delta")
                {
                    digit4 = "3";
                }
                else
                {
                    digit4 = "5";
                }
            }
            else
            {
                if(quadrant == "Gamma" || quadrant == "Delta")
                {
                    digit4 = "1";
                }
                else
                {
                    digit4 = "7";
                }
            }
        }
        else if (Bomb.GetBatteryCount() >= 8)
        {
            if(correctRank == "Lieutenant Commander" || correctRank == "Commander" || correctRank == "Captain")
            {
                if(quadrant == "Gamma" || quadrant == "Delta")
                {
                    digit4 = "4";
                }
                else
                {
                    digit4 = "9";
                }
            }
            else
            {
                if(quadrant == "Alpha" || quadrant == "Delta")
                {
                    digit4 = "2";
                }
                else
                {
                    digit4 = digit2;
                }
            }
        }
        else
        {
            if(correctRank == "Ensign" || correctRank == "Crewman")
            {
                if(quadrant == "Alpha" || quadrant == "Gamma")
                {
                    digit4 = "0";
                }
                else
                {
                    digit4 = digit1;
                }
            }
            else
            {
                if(quadrant == "Alpha" || quadrant == "Gamma")
                {
                    digit4 = "6";
                }
                else
                {
                    digit4 = "8";
                }
            }
        }
        correctEncryption = (digit1 + digit2 + digit3 + digit4);
    }


    void onEngageButton()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if (displayedScreen[4] == false)
        {
            switchOffScreenBools();
            displayedScreen[4] = true;
            engageObjects.SetActive(true);
            animator[0].SetBool("buttonOn", false);
            animator[0].SetBool("buttonOff", true);
            animator[1].SetBool("buttonOn", false);
            animator[1].SetBool("buttonOff", true);
            animator[2].SetBool("buttonOn", false);
            animator[2].SetBool("buttonOff", true);
            animator[3].SetBool("buttonOn", false);
            animator[3].SetBool("buttonOff", true);
            animator[4].SetBool("buttonOn", true);
            animator[4].SetBool("buttonOff", false);
        }
        else
        {
            animator[4].SetBool("buttonOn", false);
            animator[4].SetBool("buttonOff", true);
            switchOffScreenBools();
        }
    }

    void onEngageSubmitButton()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if(moduleSolved)
        {
            return;
        }
        else if(engageActive == false && engageOperation == false && destructActive == false && destructionOperation == false)
        {
            engageActive = true;
            Audio.PlaySoundAtTransform("inputReceived", transform);
            StartCoroutine(engageFinal());
            engageSubmitButtonText.text = "Disengage";
            Debug.LogFormat("[Lightspeed #{0}] Engines engaged.", moduleId);
        }
        else if(engageOperation == false && destructActive == false && destructionOperation == false)
        {
            engageActive = false;
            Audio.PlaySoundAtTransform("cancelEngage", transform);
            StartCoroutine(cancelEngage());
            engageSubmitButtonText.text = "Engage";
            Debug.LogFormat("[Lightspeed #{0}] Engines disengaged.", moduleId);
        }
    }

    IEnumerator engageFinal()
    {
        Audio.PlaySoundAtTransform("warpAlarm", transform);
        yield return new WaitForSeconds(2f);
        if (engageActive == false)
        {
	        yield break;
		}

	    for (int i = 9; i >= 0; i--)
	    {
			if(i % 2 == 0) Audio.PlaySoundAtTransform("warpAlarm", transform);
		    countdownBars[i].material = countdownMat[1];
		    yield return new WaitForSeconds(0.5f);
		    if (engageActive == false)
		    {
			    yield break;
		    }
		}

	    if(warpDisplayText.text == correctWarpSpeed.ToString() && destinationDisplayText.text == correctPlanet && submittedOfficer == correctCrew && dataEncrypted)
	    {
		    GetComponent<KMBombModule>().HandlePass();
		    Audio.PlaySoundAtTransform("warp", transform);
		    Debug.LogFormat("[Lightspeed #{0}] Module solved. Live long and prosper.", moduleId);
		    moduleSolved = true;
	    }
	    else
	    {
		    GetComponent<KMBombModule>().HandleStrike();
		    Debug.LogFormat("[Lightspeed #{0}] Warning: Input incorrect. Warp drive was set to {1}. Destination was set to {2}. Ranking officer was set to {3}. Data encryption was {4}.", moduleId, warpDisplayText.text, destinationDisplayText.text, submittedOfficer, dataEncrypted);
		    Audio.PlaySoundAtTransform("cancelEngage", transform);
		    engageSubmitButtonText.text = "Engage";
		    StartCoroutine(cancelEngage());
		    engageActive = false;
	    }
    }

    IEnumerator cancelEngage()
    {
        engageOperation = true;
	    yield return new WaitForSeconds(0.1f);
		for (int i = 0; i < 10; i++)
	    {
		    countdownBars[i].material = countdownMat[0];
		    yield return new WaitForSeconds(0.1f);
		}
        engageOperation = false;
    }

    void onDestructButton()
    {
        GetComponent<KMSelectable>().AddInteractionPunch(0.5f);
        soundIndex = UnityEngine.Random.Range(1,3);
        Audio.PlaySoundAtTransform(beep[soundIndex].name, transform);
        if(destructActive == false && destructionOperation == false && engageActive == false & engageOperation == false)
        {
            destructActive = true;
            Audio.PlaySoundAtTransform("destructActive", transform);
            StartCoroutine(selfDestruct());
            destructButtonText.text = "Cancel auto\ndestruct sequence";
            Debug.LogFormat("[Lightspeed #{0}] Warning: Auto destruct sequence activated.", moduleId);
        }
        else if(destructionOperation == false && engageActive == false && engageOperation == false)
        {
            destructActive = false;
	        destructByTwitch = false;
            Audio.PlaySoundAtTransform("destructCancel", transform);
            StartCoroutine(cancelDestruct());
            destructButtonText.text = "Activate auto\ndestruct sequence";
            Debug.LogFormat("[Lightspeed #{0}] Auto destruct sequence cancelled.", moduleId);
        }
    }

    IEnumerator selfDestruct()
    {
        Audio.PlaySoundAtTransform("destructAlarm", transform);
        yield return new WaitForSeconds(2f);
        if (destructActive == false)
        {
	        yield break;
        }

	    for (int i = 0; i < 10; i++)
	    {
		    Audio.PlaySoundAtTransform("destructAlarm", transform);
		    countdownBars[i].material = countdownMat[2];
		    yield return new WaitForSeconds(1.5f);
		    if (destructActive == false)
		    {
			    yield break;
		    }
		}

	    Debug.LogFormat("[Lightspeed #{0}] WARP CORE OVERLOAD. SHIP DESTROYED.", moduleId);
	    while(!shipDestroyed && !destructByTwitch)
	    {
		    GetComponent<KMBombModule>().HandleStrike();
	    }
    }

    IEnumerator cancelDestruct()
    {
        destructionOperation = true;
        yield return new WaitForSeconds(0.1f);
	    for (int i = 9; i >= 0; i--)
	    {
		    countdownBars[i].material = countdownMat[0];
		    yield return new WaitForSeconds(0.1f);
	    }
        destructionOperation = false;
    }

    void setStatusLevels()
    {
        antimatterLevel = UnityEngine.Random.Range(40,100);
        dilithiumLevel = UnityEngine.Random.Range(40,100);
        shieldsLevel = UnityEngine.Random.Range(20,100);
        antimatterText.text = antimatterLevel.ToString() + "%";
        dilithiumText.text = dilithiumLevel.ToString() + "%";
        shieldsText.text = shieldsLevel.ToString() + "%";
        antimatterBar.transform.localScale = new Vector3(1, 1, (float)(antimatterLevel / 100f));
        dilithiumBar.transform.localScale = new Vector3(1, 1, (float)(dilithiumLevel / 100f));
        shieldsBar.transform.localScale = new Vector3(1, 1, (float)(shieldsLevel / 100f));
        stardate = UnityEngine.Random.Range(34127,87654);
        subStardate = UnityEngine.Random.Range(0,10);
        stardateText.text = stardate.ToString() + "." + subStardate.ToString();
    }

    void setQuadrant()
    {
        int symbolIndex = UnityEngine.Random.Range(0,3);
        symbol = symbolOptions[symbolIndex];
        int colorIndex = UnityEngine.Random.Range(0,3);
        symbolColor = colorOptions[colorIndex];
        symbolText.text = symbol;
        symbolText.color = symbolColor;
        int quadrantIndex = UnityEngine.Random.Range(0,4);
        quadrant = quadrantOptions[quadrantIndex];
        foreach(Renderer point in quadrantPoint)
        {
            point.material = quadrantInactive;
        }
        if(symbolIndex == 0)
        {
            if(colorIndex == 0)
            {
                if(quadrantIndex == 0)
                {
                    quadrantPoint[0].material = quadrantActive;
                }
                else if(quadrantIndex == 1)
                {
                    quadrantPoint[1].material = quadrantActive;
                }
                else if(quadrantIndex == 2)
                {
                    quadrantPoint[2].material = quadrantActive;
                }
                else if(quadrantIndex == 3)
                {
                    quadrantPoint[3].material = quadrantActive;
                }
            }
            else if(colorIndex == 1)
            {
                if(quadrantIndex == 0)
                {
                    quadrantPoint[0].material = quadrantActive;
                }
                else if(quadrantIndex == 1)
                {
                    quadrantPoint[3].material = quadrantActive;
                }
                else if(quadrantIndex == 2)
                {
                    quadrantPoint[2].material = quadrantActive;
                }
                else if(quadrantIndex == 3)
                {
                    quadrantPoint[1].material = quadrantActive;
                }
            }
            else if(colorIndex == 2)
            {
                if(quadrantIndex == 0)
                {
                    quadrantPoint[3].material = quadrantActive;
                }
                else if(quadrantIndex == 1)
                {
                    quadrantPoint[2].material = quadrantActive;
                }
                else if(quadrantIndex == 2)
                {
                    quadrantPoint[0].material = quadrantActive;
                }
                else if(quadrantIndex == 3)
                {
                    quadrantPoint[1].material = quadrantActive;
                }
            }
        }
        else if(symbolIndex == 1)
        {
            if(colorIndex == 0)
            {
                if(quadrantIndex == 0)
                {
                    quadrantPoint[3].material = quadrantActive;
                }
                else if(quadrantIndex == 1)
                {
                    quadrantPoint[0].material = quadrantActive;
                }
                else if(quadrantIndex == 2)
                {
                    quadrantPoint[1].material = quadrantActive;
                }
                else if(quadrantIndex == 3)
                {
                    quadrantPoint[2].material = quadrantActive;
                }
            }
            else if(colorIndex == 1)
            {
                if(quadrantIndex == 0)
                {
                    quadrantPoint[1].material = quadrantActive;
                }
                else if(quadrantIndex == 1)
                {
                    quadrantPoint[0].material = quadrantActive;
                }
                else if(quadrantIndex == 2)
                {
                    quadrantPoint[3].material = quadrantActive;
                }
                else if(quadrantIndex == 3)
                {
                    quadrantPoint[2].material = quadrantActive;
                }
            }
            else if(colorIndex == 2)
            {
                if(quadrantIndex == 0)
                {
                    quadrantPoint[3].material = quadrantActive;
                }
                else if(quadrantIndex == 1)
                {
                    quadrantPoint[2].material = quadrantActive;
                }
                else if(quadrantIndex == 2)
                {
                    quadrantPoint[1].material = quadrantActive;
                }
                else if(quadrantIndex == 3)
                {
                    quadrantPoint[0].material = quadrantActive;
                }
            }
        }
        else if(symbolIndex == 2)
        {
            if(colorIndex == 0)
            {
                if(quadrantIndex == 0)
                {
                    quadrantPoint[1].material = quadrantActive;
                }
                else if(quadrantIndex == 1)
                {
                    quadrantPoint[3].material = quadrantActive;
                }
                else if(quadrantIndex == 2)
                {
                    quadrantPoint[2].material = quadrantActive;
                }
                else if(quadrantIndex == 3)
                {
                    quadrantPoint[0].material = quadrantActive;
                }
            }
            else if(colorIndex == 1)
            {
                if(quadrantIndex == 0)
                {
                    quadrantPoint[2].material = quadrantActive;
                }
                else if(quadrantIndex == 1)
                {
                    quadrantPoint[1].material = quadrantActive;
                }
                else if(quadrantIndex == 2)
                {
                    quadrantPoint[3].material = quadrantActive;
                }
                else if(quadrantIndex == 3)
                {
                    quadrantPoint[0].material = quadrantActive;
                }
            }
            else if(colorIndex == 2)
            {
                if(quadrantIndex == 0)
                {
                    quadrantPoint[2].material = quadrantActive;
                }
                else if(quadrantIndex == 1)
                {
                    quadrantPoint[0].material = quadrantActive;
                }
                else if(quadrantIndex == 2)
                {
                    quadrantPoint[3].material = quadrantActive;
                }
                else if(quadrantIndex == 3)
                {
                    quadrantPoint[1].material = quadrantActive;
                }
            }
        }
    }

    void switchOffScreenBools()
    {
        displayedScreen[0] = false;
        displayedScreen[1] = false;
        displayedScreen[2] = false;
        displayedScreen[3] = false;
        displayedScreen[4] = false;
        warpObjects.SetActive(false);
        destinationObjects.SetActive(false);
        officerObjects.SetActive(false);
        encryptObjects.SetActive(false);
        engageObjects.SetActive(false);
    }

    IEnumerator screenAnim()
    {
        foreach (Renderer dots in topDots)
        {
            dots.material = lightOff;
        }
        foreach (Renderer dots in bottomDots)
        {
            dots.material = lightOff;
        }

        while (screenAnimatorOn)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (Renderer dots in topDots)
            {
                dots.material = lightOff;
            }
            foreach (Renderer dots in bottomDots)
            {
                dots.material = lightOff;
            }
            if (litDots == 1)
            {
                topDots[0].material = lightOn;
                bottomDots[0].material = lightOn;
                litDots++;
            }
            else if (litDots == 2)
            {
                topDots[1].material = lightOn;
                topDots[2].material = lightOn;
                bottomDots[1].material = lightOn;
                bottomDots[2].material = lightOn;
                litDots++;
            }
            else if (litDots == 3)
            {
                topDots[3].material = lightOn;
                topDots[4].material = lightOn;
                bottomDots[3].material = lightOn;
                bottomDots[4].material = lightOn;
                litDots++;
            }
            else if (litDots == 4)
            {
                topDots[5].material = lightOn;
                topDots[6].material = lightOn;
                bottomDots[5].material = lightOn;
                bottomDots[6].material = lightOn;
                litDots++;
            }
            else if (litDots == 5)
            {
                topDots[7].material = lightOn;
                topDots[8].material = lightOn;
                bottomDots[7].material = lightOn;
                bottomDots[8].material = lightOn;
                litDots++;
            }
            else if (litDots == 6)
            {
                topDots[9].material = lightOn;
                topDots[10].material = lightOn;
                bottomDots[9].material = lightOn;
                bottomDots[10].material = lightOn;
                litDots = 1;
            }
        }
    }
}
