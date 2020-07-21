using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class CharacterCreator : MonoBehaviour
    {
        [Header("Material")]
        public Material mat;

        [Header("Skin Colors")]
        public Color whiteSkin =  new Color(1f, 0.8000001f, 0.682353f);
        public Color brownSkin =  new Color(0.8196079f, 0.6352941f, 0.4588236f);
        public Color blackSkin =  new Color(0.5647059f, 0.4078432f, 0.3137255f);
        public Color elfSkin =  new Color(0.9607844f, 0.7843138f, 0.7294118f);

        [Header("Scar Colors")]
        public Color whiteScar = new Color(0.9294118f, 0.6862745f, 0.5921569f);
        public Color brownScar = new Color(0.6980392f, 0.5450981f, 0.4f);
        public Color blackScar = new Color(0.4235294f, 0.3176471f, 0.282353f);
        public Color elfScar = new Color(0.8745099f, 0.6588235f, 0.6313726f);

        [Header("Stubble Colors")]
        public Color whiteStubble = new Color(0.8039216f, 0.7019608f, 0.6313726f);
        public Color brownStubble = new Color(0.6588235f, 0.572549f, 0.4627451f);
        public Color blackStubble = new Color(0.3882353f, 0.2901961f, 0.2470588f);
        public Color elfStubble = new Color(0.8627452f, 0.7294118f, 0.6862745f);

        [Header("Body Art Colors")]
        public Color[] bodyArt = { new Color(0.0509804f, 0.6745098f, 0.9843138f), new Color(0.7215686f, 0.2666667f, 0.2666667f)};

        // character object lists
        // male list
        [HideInInspector]
        public CharacterObjectGroups male;

        // female list
        [HideInInspector]
        public CharacterObjectGroups female;

        // universal list
        [HideInInspector]
        public CharacterObjectListsAllGender allGender;

        private Gender gender = Gender.Male;

        //holds the acttive bits, should be useful for saving
        private int[] equipped;
        private int maxParts = 0;

        //dictionary to keep all posible items for fast reference
        private List<GameObject>[,] allObjects;

        #region Init and Helpers
        // Start is called before the first frame update
        void Start()
        {
            //set up the vectors
            maxParts = Enum.GetValues(typeof(BodyParts)).Length;
            equipped = new int[maxParts];

            //silly C# sintax
            allObjects = new List<GameObject>[2, maxParts];

            // rebuild all lists
            BuildLists();

            //set to -1 which means nothing equipped
            for (int i = 0; i < maxParts; i++)
            {
                equipped[i] = -1;
            }

            //default startup as male
            equipped[(int)BodyParts.HeadAllElements] = 0;
            equipped[(int)BodyParts.Eyebrow] = 0;
            equipped[(int)BodyParts.Torso] = 0;
            equipped[(int)BodyParts.Arm_Upper_Right] = 0;
            equipped[(int)BodyParts.Arm_Upper_Left] = 0;
            equipped[(int)BodyParts.Arm_Lower_Right] = 0;
            equipped[(int)BodyParts.Arm_Lower_Left] = 0;
            equipped[(int)BodyParts.Hand_Right] = 0;
            equipped[(int)BodyParts.Hand_Left] = 0;
            equipped[(int)BodyParts.Hips] = 0;
            equipped[(int)BodyParts.Leg_Right] = 0;
            equipped[(int)BodyParts.Leg_Left] = 0;

            EnableCharacter();
        }

        private void EnableCharacter()
        {
            //activate the look
            for (int i = 0; i < maxParts; i++)
            {
                ActivateItem(i, equipped[i]);
            }
        }
        private void DisableCharacter()
        {
            //activate the look
            for (int i = 0; i < maxParts; i++)
            {
                DeactivateItem(i, equipped[i]);
            }
        }

        private void DeactivateItem(int itemType, int itemIndex)
        {
            //if we had a previous item 
            if (equipped[itemType] != -1 && equipped[itemType] < allObjects[(int)gender, itemType].Count)
            {
                allObjects[(int)gender, itemType][equipped[itemType]].SetActive(false);
            }
        }

        // enable game object and add it to the enabled objects list
        void ActivateItem(int itemType, int itemIndex)
        {
            if (itemIndex >= allObjects[(int)gender, itemType].Count)
            {
                itemIndex = -1;
            }
            //if we had a previous item 
            if (equipped[itemType] != -1)
            {
                if (equipped[itemType] < allObjects[(int)gender, itemType].Count)
                {
                    allObjects[(int)gender, itemType][equipped[itemType]].SetActive(false);
                }
                if (itemIndex != -1)
                {
                    equipped[itemType] = itemIndex;
                    allObjects[(int)gender, itemType][equipped[itemType]].SetActive(true);
                }

            }
            else
            {
                if (itemIndex != -1)
                {
                    equipped[itemType] = itemIndex;
                    allObjects[(int)gender, itemType][equipped[itemType]].SetActive(true);
                }
            }

            
        }

        Color ConvertColor(int r, int g, int b)
        {
            return new Color(r / 255.0f, g / 255.0f, b / 255.0f, 1);
        }
        #endregion

        #region UI Setters
        public void SetHead(int index, string name)
        {
            if (index < allObjects[(int)gender, (int)BodyParts.HeadAllElements].Count)
            {
                ActivateItem((int)BodyParts.HeadAllElements, index);
            }
        }

        public void SetEyebrows(int index, string name)
        {
            if (index < allObjects[(int)gender, (int)BodyParts.Eyebrow].Count)
            {
                ActivateItem((int)BodyParts.Eyebrow, index);
            }
        }

        public void SetHair(int index, string name)
        {
            //handle no hair option
            index--;
            if(index < allObjects[(int)gender, (int)BodyParts.All_Hair].Count)
            {
                ActivateItem((int)BodyParts.All_Hair, index);
            }
        }

        public void SetFacialHair(int index, string name)
        {
            if (gender == Gender.Female)
                return;
            //handle no hair option
            index--;
            if (index < allObjects[(int)gender, (int)BodyParts.FacialHair].Count)
            {
                ActivateItem((int)BodyParts.FacialHair, index);
            }
        }

        public void SetRace(string name, Color color)
        {
            Color skinColor = Color.black;
            Color scarColor = Color.black;
            Color stubbleColor = Color.black;
            switch (name)
            {
                case "Race 1": //Human
                    skinColor = whiteSkin;
                    scarColor = whiteScar;
                    stubbleColor = whiteStubble;
                    break;
                case "Race 2":
                    skinColor = brownSkin;
                    scarColor = brownScar;
                    stubbleColor = brownStubble;
                    break;
                case "Race 3":
                    skinColor = blackSkin;
                    scarColor = blackScar;
                    stubbleColor = blackStubble;
                    break;
                case "Race 4":
                    skinColor = elfSkin;
                    scarColor = elfScar;
                    stubbleColor = elfStubble;
                    //activate elf ears
                    break;

            }
            //set the skin color
            mat.SetColor("_Color_Skin", skinColor);
            
            // set stubble color
            mat.SetColor("_Color_Stubble", stubbleColor);

            // set scar color
            mat.SetColor("_Color_Scar", scarColor);
        }

        public void SetHairColor(string name, Color color)
        {
            mat.SetColor("_Color_Hair", color);
        }

        public void SetGender(string name, Color color)
        {
            Gender newGender;
            //string comparison yes!
            if(name.CompareTo("Gender (Male)") == 0)
            {
                newGender = Gender.Male;
            }
            else
            {
                newGender = Gender.Female;
            }
            if (newGender != gender)
            {
                DisableCharacter();
                gender = newGender;
                EnableCharacter();
            }
        }
        #endregion



        #region Builders
        // build all item lists for use in randomization
        private void BuildLists()
        {
            //build out male lists
            BuildList(male.headAllElements, "Male_Head_All_Elements");
            BuildList(male.headNoElements, "Male_Head_No_Elements");
            BuildList(male.eyebrow, "Male_01_Eyebrows");
            BuildList(male.facialHair, "Male_02_FacialHair");
            BuildList(male.torso, "Male_03_Torso");
            BuildList(male.arm_Upper_Right, "Male_04_Arm_Upper_Right");
            BuildList(male.arm_Upper_Left, "Male_05_Arm_Upper_Left");
            BuildList(male.arm_Lower_Right, "Male_06_Arm_Lower_Right");
            BuildList(male.arm_Lower_Left, "Male_07_Arm_Lower_Left");
            BuildList(male.hand_Right, "Male_08_Hand_Right");
            BuildList(male.hand_Left, "Male_09_Hand_Left");
            BuildList(male.hips, "Male_10_Hips");
            BuildList(male.leg_Right, "Male_11_Leg_Right");
            BuildList(male.leg_Left, "Male_12_Leg_Left");

            //add them to the male list
            allObjects[0, (int)BodyParts.HeadAllElements] = male.headAllElements;
            allObjects[0, (int)BodyParts.HeadNoElements] = male.headNoElements;
            allObjects[0, (int)BodyParts.Eyebrow] = male.eyebrow;
            allObjects[0, (int)BodyParts.FacialHair] = male.facialHair;
            allObjects[0, (int)BodyParts.Torso] = male.torso;
            allObjects[0, (int)BodyParts.Arm_Upper_Right] = male.arm_Upper_Right;
            allObjects[0, (int)BodyParts.Arm_Upper_Left] = male.arm_Upper_Left;
            allObjects[0, (int)BodyParts.Arm_Lower_Right] = male.arm_Lower_Right;
            allObjects[0, (int)BodyParts.Arm_Lower_Left] = male.arm_Lower_Left;
            allObjects[0, (int)BodyParts.Hand_Right] = male.hand_Right;
            allObjects[0, (int)BodyParts.Hand_Left] = male.hand_Left;
            allObjects[0, (int)BodyParts.Hips] = male.hips;
            allObjects[0, (int)BodyParts.Leg_Right] = male.leg_Right;
            allObjects[0, (int)BodyParts.Leg_Left] = male.leg_Left;

            //build out female lists
            BuildList(female.headAllElements, "Female_Head_All_Elements");
            BuildList(female.headNoElements, "Female_Head_No_Elements");
            BuildList(female.eyebrow, "Female_01_Eyebrows");
            BuildList(female.facialHair, "Female_02_FacialHair");
            BuildList(female.torso, "Female_03_Torso");
            BuildList(female.arm_Upper_Right, "Female_04_Arm_Upper_Right");
            BuildList(female.arm_Upper_Left, "Female_05_Arm_Upper_Left");
            BuildList(female.arm_Lower_Right, "Female_06_Arm_Lower_Right");
            BuildList(female.arm_Lower_Left, "Female_07_Arm_Lower_Left");
            BuildList(female.hand_Right, "Female_08_Hand_Right");
            BuildList(female.hand_Left, "Female_09_Hand_Left");
            BuildList(female.hips, "Female_10_Hips");
            BuildList(female.leg_Right, "Female_11_Leg_Right");
            BuildList(female.leg_Left, "Female_12_Leg_Left");

            //add them to the female list
            allObjects[1, (int)BodyParts.HeadAllElements] = female.headAllElements;
            allObjects[1, (int)BodyParts.HeadNoElements] = female.headNoElements;
            allObjects[1, (int)BodyParts.Eyebrow] = female.eyebrow;
            allObjects[1, (int)BodyParts.FacialHair] = female.facialHair;
            allObjects[1, (int)BodyParts.Torso] = female.torso;
            allObjects[1, (int)BodyParts.Arm_Upper_Right] = female.arm_Upper_Right;
            allObjects[1, (int)BodyParts.Arm_Upper_Left] = female.arm_Upper_Left;
            allObjects[1, (int)BodyParts.Arm_Lower_Right] = female.arm_Lower_Right;
            allObjects[1, (int)BodyParts.Arm_Lower_Left] = female.arm_Lower_Left;
            allObjects[1, (int)BodyParts.Hand_Right] = female.hand_Right;
            allObjects[1, (int)BodyParts.Hand_Left] = female.hand_Left;
            allObjects[1, (int)BodyParts.Hips] = female.hips;
            allObjects[1, (int)BodyParts.Leg_Right] = female.leg_Right;
            allObjects[1, (int)BodyParts.Leg_Left] = female.leg_Left;

            // build out all gender lists
            BuildList(allGender.all_Hair, "All_01_Hair");
            BuildList(allGender.all_Head_Attachment, "All_02_Head_Attachment");
            BuildList(allGender.headCoverings_Base_Hair, "HeadCoverings_Base_Hair");
            BuildList(allGender.headCoverings_No_FacialHair, "HeadCoverings_No_FacialHair");
            BuildList(allGender.headCoverings_No_Hair, "HeadCoverings_No_Hair");
            BuildList(allGender.chest_Attachment, "All_03_Chest_Attachment");
            BuildList(allGender.back_Attachment, "All_04_Back_Attachment");
            BuildList(allGender.shoulder_Attachment_Right, "All_05_Shoulder_Attachment_Right");
            BuildList(allGender.shoulder_Attachment_Left, "All_06_Shoulder_Attachment_Left");
            BuildList(allGender.elbow_Attachment_Right, "All_07_Elbow_Attachment_Right");
            BuildList(allGender.elbow_Attachment_Left, "All_08_Elbow_Attachment_Left");
            BuildList(allGender.hips_Attachment, "All_09_Hips_Attachment");
            BuildList(allGender.knee_Attachement_Right, "All_10_Knee_Attachement_Right");
            BuildList(allGender.knee_Attachement_Left, "All_11_Knee_Attachement_Left");
            BuildList(allGender.elf_Ear, "Elf_Ear");

            //add common parts to both lists
            allObjects[0, (int)BodyParts.All_Hair] = allGender.all_Hair;
            allObjects[0, (int)BodyParts.All_Head_Attachment] = allGender.all_Head_Attachment;
            allObjects[0, (int)BodyParts.HeadCoverings_Base_Hair] = allGender.headCoverings_Base_Hair;
            allObjects[0, (int)BodyParts.HeadCoverings_No_FacialHair] = allGender.headCoverings_No_FacialHair;
            allObjects[0, (int)BodyParts.HeadCoverings_No_Hair] = allGender.headCoverings_No_Hair;
            allObjects[0, (int)BodyParts.Chest_Attachment] = allGender.chest_Attachment;
            allObjects[0, (int)BodyParts.Back_Attachment] = allGender.back_Attachment;
            allObjects[0, (int)BodyParts.Shoulder_Attachment_Right] = allGender.shoulder_Attachment_Right;
            allObjects[0, (int)BodyParts.Shoulder_Attachment_Left] = allGender.shoulder_Attachment_Left;
            allObjects[0, (int)BodyParts.Elbow_Attachment_Right] = allGender.elbow_Attachment_Right;
            allObjects[0, (int)BodyParts.Elbow_Attachment_Left] = allGender.elbow_Attachment_Left;
            allObjects[0, (int)BodyParts.Hips_Attachment] = allGender.hips_Attachment;
            allObjects[0, (int)BodyParts.Knee_Attachement_Right] = allGender.knee_Attachement_Right;
            allObjects[0, (int)BodyParts.Knee_Attachement_Left] = allGender.knee_Attachement_Left;
            allObjects[0, (int)BodyParts.Elf_Ear] = allGender.elf_Ear;

            //add common parts to both lists
            allObjects[1, (int)BodyParts.All_Hair] = allGender.all_Hair;
            allObjects[1, (int)BodyParts.All_Head_Attachment] = allGender.all_Head_Attachment;
            allObjects[1, (int)BodyParts.HeadCoverings_Base_Hair] = allGender.headCoverings_Base_Hair;
            allObjects[1, (int)BodyParts.HeadCoverings_No_FacialHair] = allGender.headCoverings_No_FacialHair;
            allObjects[1, (int)BodyParts.HeadCoverings_No_Hair] = allGender.headCoverings_No_Hair;
            allObjects[1, (int)BodyParts.Chest_Attachment] = allGender.chest_Attachment;
            allObjects[1, (int)BodyParts.Back_Attachment] = allGender.back_Attachment;
            allObjects[1, (int)BodyParts.Shoulder_Attachment_Right] = allGender.shoulder_Attachment_Right;
            allObjects[1, (int)BodyParts.Shoulder_Attachment_Left] = allGender.shoulder_Attachment_Left;
            allObjects[1, (int)BodyParts.Elbow_Attachment_Right] = allGender.elbow_Attachment_Right;
            allObjects[1, (int)BodyParts.Elbow_Attachment_Left] = allGender.elbow_Attachment_Left;
            allObjects[1, (int)BodyParts.Hips_Attachment] = allGender.hips_Attachment;
            allObjects[1, (int)BodyParts.Knee_Attachement_Right] = allGender.knee_Attachement_Right;
            allObjects[1, (int)BodyParts.Knee_Attachement_Left] = allGender.knee_Attachement_Left;
            allObjects[1, (int)BodyParts.Elf_Ear] = allGender.elf_Ear;

        }

        // called from the BuildLists method
        void BuildList(List<GameObject> targetList, string characterPart)
        {
            Transform[] rootTransform = gameObject.GetComponentsInChildren<Transform>();

            // declare target root transform
            Transform targetRoot = null;

            // find character parts parent object in the scene
            foreach (Transform t in rootTransform)
            {
                if (t.gameObject.name == characterPart)
                {
                    targetRoot = t;
                    break;
                }
            }

            // clears targeted list of all objects
            targetList.Clear();

            // cycle through all child objects of the parent object
            for (int i = 0; i < targetRoot.childCount; i++)
            {
                // get child gameobject index i
                GameObject go = targetRoot.GetChild(i).gameObject;

                // disable child object
                go.SetActive(false);

                // add object to the targeted object list
                targetList.Add(go);

                // collect the material for the random character, only if null in the inspector;
                if (!mat)
                {
                    if (go.GetComponent<SkinnedMeshRenderer>())
                        mat = go.GetComponent<SkinnedMeshRenderer>().material;
                }
                else
                {
                    if (go.GetComponent<SkinnedMeshRenderer>())
                    {
                        go.GetComponent<SkinnedMeshRenderer>().material = mat;
                    }
                }
            }
        }
        #endregion
    }
}
