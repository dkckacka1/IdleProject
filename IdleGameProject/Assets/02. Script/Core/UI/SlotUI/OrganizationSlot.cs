using System;
using UnityEngine;

namespace IdleProject.Core.UI.Slot
{
    public class OrganizationSlot : SlotComponent
    {
        [SerializeField] private GameObject organizationObject;

        public void SetOrganization(bool isOrganization)
        {
            organizationObject.SetActive(isOrganization);
        }
    }
}
