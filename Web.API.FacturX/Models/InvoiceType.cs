﻿namespace LIB_ZUGFeRD.Models {

    public enum InvoiceType
    {
        Unknown = 0,
        Invoice = 380,
        Correction = 1380,
        CreditNote = 381,
        DebitNote = 383,
        SelfBilledInvoice = 389
    }
}
