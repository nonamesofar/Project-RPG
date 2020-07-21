using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public enum Gender { Male, Female }
    public enum Race { Human, Elf }
    public enum SkinColor { White, Brown, Black, Elf }
    public enum Elements { Yes, No }
    public enum HeadCovering { HeadCoverings_Base_Hair, HeadCoverings_No_FacialHair, HeadCoverings_No_Hair }
    public enum FacialHair { Yes, No }

    public enum BodyParts
    {
        //male and female parts
        HeadAllElements,
        HeadNoElements,
        Eyebrow,
        FacialHair,
        Torso,
        Arm_Upper_Right,
        Arm_Upper_Left,
        Arm_Lower_Right,
        Arm_Lower_Left,
        Hand_Right,
        Hand_Left,
        Hips,
        Leg_Right,
        Leg_Left,

        //all the extra bits
        HeadCoverings_Base_Hair,
        HeadCoverings_No_FacialHair,
        HeadCoverings_No_Hair,
        All_Hair,
        All_Head_Attachment,
        Chest_Attachment,
        Back_Attachment,
        Shoulder_Attachment_Right,
        Shoulder_Attachment_Left,
        Elbow_Attachment_Right,
        Elbow_Attachment_Left,
        Hips_Attachment,
        Knee_Attachement_Right,
        Knee_Attachement_Left,
        //All_12_Extra, //?????
        Elf_Ear
    }


    // classe for keeping the lists organized, allows for simple switching from male/female objects
    [System.Serializable]
    public class CharacterObjectGroups
    {
        public List<GameObject> headAllElements;
        public List<GameObject> headNoElements;
        public List<GameObject> eyebrow;
        public List<GameObject> facialHair;
        public List<GameObject> torso;
        public List<GameObject> arm_Upper_Right;
        public List<GameObject> arm_Upper_Left;
        public List<GameObject> arm_Lower_Right;
        public List<GameObject> arm_Lower_Left;
        public List<GameObject> hand_Right;
        public List<GameObject> hand_Left;
        public List<GameObject> hips;
        public List<GameObject> leg_Right;
        public List<GameObject> leg_Left;
    }

    // classe for keeping the lists organized, allows for organization of the all gender items
    [System.Serializable]
    public class CharacterObjectListsAllGender
    {
        public List<GameObject> headCoverings_Base_Hair;
        public List<GameObject> headCoverings_No_FacialHair;
        public List<GameObject> headCoverings_No_Hair;
        public List<GameObject> all_Hair;
        public List<GameObject> all_Head_Attachment;
        public List<GameObject> chest_Attachment;
        public List<GameObject> back_Attachment;
        public List<GameObject> shoulder_Attachment_Right;
        public List<GameObject> shoulder_Attachment_Left;
        public List<GameObject> elbow_Attachment_Right;
        public List<GameObject> elbow_Attachment_Left;
        public List<GameObject> hips_Attachment;
        public List<GameObject> knee_Attachement_Right;
        public List<GameObject> knee_Attachement_Left;
        public List<GameObject> all_12_Extra;
        public List<GameObject> elf_Ear;
    }
}
