using System;
using UnityEngine;

namespace IdleProject.Core.UI.Slot
{
    public class OrganizationSlot : SlotParts
    {
        [SerializeField] private GameObject organizationObject;

        public void SetOrganization(bool isOrganization)
        {
            organizationObject.SetActive(isOrganization);
        }
    }
}
