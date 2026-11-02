using bSpine.Models;

namespace bSpine.Data;

/// <summary>
/// Fictional demo data only. No real people, providers or PHI.
/// Dates are relative to "today" so deadline warnings are always visible.
/// </summary>
public static class SeedData
{
    public static void Initialize(IServiceProvider services)
    {
        var db = services.GetRequiredService<BSpineDbContext>();
        db.Database.EnsureCreated();

        if (db.Clients.Any())
        {
            return;
        }

        var today = DateTime.Today;

        // ---------- Clients ----------
        var marcus = new Client
        {
            FirstName = "Marcus", LastName = "Alvarez", DateOfBirth = new DateTime(1984, 3, 14),
            Phone = "555-0101", Email = "marcus.alvarez@example.com", Address = "412 Maple Ave, Springfield",
            InjuryDate = today.AddDays(-320), InjuryType = "Lumbar disc herniation (L4-L5)",
            InjurySummary = "Rear-end collision; radiating lower back and leg pain."
        };
        var priya = new Client
        {
            FirstName = "Priya", LastName = "Natarajan", DateOfBirth = new DateTime(1991, 7, 2),
            Phone = "555-0102", Email = "priya.n@example.com", Address = "88 Lakeview Dr, Riverton",
            InjuryDate = today.AddDays(-150), InjuryType = "Cervical radiculopathy (C5-C6)",
            InjurySummary = "Slip and fall at a retail store; neck pain with arm numbness."
        };
        var daniel = new Client
        {
            FirstName = "Daniel", LastName = "O'Connor", DateOfBirth = new DateTime(1976, 11, 21),
            Phone = "555-0103", Email = "d.oconnor@example.com", Address = "7 Harbor St, Fairview",
            InjuryDate = today.AddDays(-640), InjuryType = "Lumbar disc herniation (L5-S1) with sciatica",
            InjurySummary = "Workplace accident involving a third-party contractor."
        };
        var hannah = new Client
        {
            FirstName = "Hannah", LastName = "Whitfield", DateOfBirth = new DateTime(1968, 5, 30),
            Phone = "555-0104", Email = "hannah.w@example.com", Address = "230 Oak Ridge Rd, Lakeside",
            InjuryDate = today.AddDays(-500), InjuryType = "Thoracic compression fracture (T11)",
            InjurySummary = "Motorcycle collision; vertebral compression fracture."
        };
        var tomasz = new Client
        {
            FirstName = "Tomasz", LastName = "Kowalski", DateOfBirth = new DateTime(1979, 1, 9),
            Phone = "555-0105", Email = "t.kowalski@example.com", Address = "19 Birch Ln, Springfield",
            InjuryDate = today.AddDays(-90), InjuryType = "Lumbar spinal stenosis",
            InjurySummary = "Construction site fall; aggravation of pre-existing condition."
        };
        var alicia = new Client
        {
            FirstName = "Alicia", LastName = "Brooks", DateOfBirth = new DateTime(1995, 9, 17),
            Phone = "555-0106", Email = "alicia.brooks@example.com", Address = "55 Cedar Ct, Riverton",
            InjuryDate = today.AddDays(-700), InjuryType = "Cervical strain with C4-C5 disc bulge",
            InjurySummary = "Intersection collision; whiplash and persistent neck pain."
        };

        // ---------- Providers ----------
        var voss = new Provider
        {
            Name = "Dr. Elena Voss - Northside Spine Institute", Type = ProviderType.SpineSurgeon,
            Specialty = "Minimally invasive spine surgery", Phone = "555-0201", Email = "records@northsidespine.example.com",
            Address = "1200 Medical Pkwy", City = "Springfield"
        };
        var kessler = new Provider
        {
            Name = "Dr. Raymond Kessler - Kessler Orthopedic Spine", Type = ProviderType.SpineSurgeon,
            Specialty = "Cervical and thoracic spine", Phone = "555-0202", Email = "info@kesslerspine.example.com",
            Address = "45 University Blvd", City = "Riverton"
        };
        var lakeshore = new Provider
        {
            Name = "Lakeshore Chiropractic", Type = ProviderType.Chiropractor,
            Specialty = "Spinal manipulation and rehab", Phone = "555-0203", Email = "front@lakeshorechiro.example.com",
            Address = "300 Shoreline Dr", City = "Lakeside"
        };
        var summit = new Provider
        {
            Name = "Summit Imaging Center", Type = ProviderType.ImagingCenter,
            Specialty = "MRI and CT", Phone = "555-0204", Email = "records@summitimaging.example.com",
            Address = "88 Summit Rd", City = "Springfield"
        };
        var harbor = new Provider
        {
            Name = "Harbor Pain Management", Type = ProviderType.PainManagement,
            Specialty = "Epidural steroid injections", Phone = "555-0205", Email = "office@harborpain.example.com",
            Address = "9 Harbor St", City = "Fairview"
        };
        var cornerstone = new Provider
        {
            Name = "Cornerstone Physical Therapy", Type = ProviderType.PhysicalTherapy,
            Specialty = "Post-injury and post-surgical rehab", Phone = "555-0206", Email = "scheduling@cornerstonept.example.com",
            Address = "610 Main St", City = "Springfield"
        };

        // ---------- Cases ----------
        var case1 = new LegalCase
        {
            CaseNumber = "BS-2026-0101", Client = marcus, AssignedAttorney = "Sarah Mitchell", Paralegal = "Laura Bennett",
            Status = CaseStatus.Treatment, OpenedDate = today.AddDays(-300), StatuteOfLimitationsDate = today.AddDays(45),
            Description = "Rear-end collision. Surgical consult pending after failed conservative care."
        };
        var case2 = new LegalCase
        {
            CaseNumber = "BS-2026-0102", Client = priya, AssignedAttorney = "James Okafor", Paralegal = "Kevin Park",
            Status = CaseStatus.Intake, OpenedDate = today.AddDays(-30), StatuteOfLimitationsDate = today.AddDays(200),
            Description = "Premises liability claim; collecting initial medical records."
        };
        var case3 = new LegalCase
        {
            CaseNumber = "BS-2026-0103", Client = daniel, AssignedAttorney = "Sarah Mitchell", Paralegal = "Kevin Park",
            Status = CaseStatus.Litigation, OpenedDate = today.AddDays(-600), StatuteOfLimitationsDate = today.AddDays(75),
            Description = "Complaint filed; discovery in progress. Surgery performed last year."
        };
        var case4 = new LegalCase
        {
            CaseNumber = "BS-2026-0104", Client = hannah, AssignedAttorney = "James Okafor", Paralegal = "Laura Bennett",
            Status = CaseStatus.Demand, OpenedDate = today.AddDays(-450), StatuteOfLimitationsDate = today.AddDays(380),
            Description = "Treatment complete; demand package being assembled."
        };
        var case5 = new LegalCase
        {
            CaseNumber = "BS-2026-0105", Client = tomasz, AssignedAttorney = "Sarah Mitchell", Paralegal = "Laura Bennett",
            Status = CaseStatus.Treatment, OpenedDate = today.AddDays(-80), StatuteOfLimitationsDate = today.AddDays(12),
            Description = "Short limitation period (public entity). Urgent filing review required."
        };
        var case6 = new LegalCase
        {
            CaseNumber = "BS-2025-0088", Client = alicia, AssignedAttorney = "James Okafor", Paralegal = "Kevin Park",
            Status = CaseStatus.Settled, OpenedDate = today.AddDays(-690), StatuteOfLimitationsDate = today.AddDays(30),
            Description = "Settled at mediation. Lien resolution outstanding."
        };

        // ---------- Treatments ----------
        var treatments = new List<Treatment>
        {
            T(case1, summit, today.AddDays(-290), "Lumbar MRI without contrast", 1850m, 0m, true),
            T(case1, lakeshore, today.AddDays(-280), "Chiropractic adjustment series (12 visits)", 2400m, 600m, true),
            T(case1, harbor, today.AddDays(-160), "Lumbar epidural steroid injection L4-L5", 4200m, 0m, true),
            T(case1, voss, today.AddDays(-40), "Surgical consultation", 650m, 0m, true),

            T(case2, summit, today.AddDays(-25), "Cervical MRI without contrast", 1750m, 0m, false),
            T(case2, kessler, today.AddDays(-14), "Orthopedic spine evaluation", 520m, 120m, false),

            T(case3, summit, today.AddDays(-560), "Lumbar MRI with and without contrast", 2300m, 450m, true),
            T(case3, harbor, today.AddDays(-500), "Transforaminal epidural injection L5-S1", 4600m, 900m, true),
            T(case3, voss, today.AddDays(-380), "Microdiscectomy L5-S1", 48500m, 6200m, true),
            T(case3, cornerstone, today.AddDays(-330), "Post-surgical physical therapy (18 visits)", 5400m, 1800m, false),

            T(case4, kessler, today.AddDays(-440), "T11 vertebral augmentation (kyphoplasty)", 36800m, 5400m, true),
            T(case4, summit, today.AddDays(-430), "Thoracic CT scan", 1400m, 1400m, false),
            T(case4, cornerstone, today.AddDays(-380), "Rehabilitation therapy (24 visits)", 7200m, 2100m, false),

            T(case5, summit, today.AddDays(-70), "Lumbar MRI", 1900m, 0m, true),
            T(case5, harbor, today.AddDays(-35), "Lumbar epidural steroid injection", 3900m, 0m, true),

            T(case6, lakeshore, today.AddDays(-650), "Chiropractic care (20 visits)", 3600m, 3600m, false),
            T(case6, kessler, today.AddDays(-600), "Cervical spine evaluation and follow-up", 1250m, 1250m, false),
            T(case6, summit, today.AddDays(-640), "Cervical MRI", 1700m, 1700m, false)
        };

        // ---------- Records requests ----------
        var requests = new List<RecordsRequest>
        {
            R(case1, voss, today.AddDays(-12), RequestStatus.Pending, today.AddDays(-2), null, "Requesting consult notes and imaging reports."),
            R(case1, lakeshore, today.AddDays(-60), RequestStatus.Received, null, today.AddDays(-45), "Full treatment log received."),
            R(case2, summit, today.AddDays(-20), RequestStatus.Pending, today.AddDays(5), null, "MRI report and films requested."),
            R(case2, kessler, today.AddDays(-10), RequestStatus.Pending, today.AddDays(10), null, "Initial evaluation records."),
            R(case3, voss, today.AddDays(-90), RequestStatus.Overdue, today.AddDays(-30), null, "Operative report still outstanding; two follow-ups made."),
            R(case3, cornerstone, today.AddDays(-50), RequestStatus.Received, null, today.AddDays(-20), "Complete PT chart received."),
            R(case4, kessler, today.AddDays(-35), RequestStatus.Pending, today.AddDays(-7), null, "Billing ledger and operative report for demand package."),
            R(case5, harbor, today.AddDays(-8), RequestStatus.Pending, today.AddDays(6), null, "Injection procedure notes."),
            R(case6, kessler, today.AddDays(-400), RequestStatus.Received, null, today.AddDays(-380), "Records received before mediation.")
        };

        // ---------- Notes ----------
        var notes = new List<CaseNote>
        {
            N(case1, "Laura Bennett", today.AddDays(-30), "Client reports pain level 7/10. Surgeon consult scheduled; need updated billing."),
            N(case1, "Sarah Mitchell", today.AddDays(-5), "Reviewed SOL: filing calendared. Considering early demand if surgical recommendation is confirmed."),
            N(case2, "Kevin Park", today.AddDays(-20), "Intake complete. Retainer signed; HIPAA authorizations on file."),
            N(case3, "Kevin Park", today.AddDays(-45), "Chased Northside Spine for operative report (second attempt)."),
            N(case3, "Sarah Mitchell", today.AddDays(-10), "Defense requested an IME. Preparing client for exam."),
            N(case4, "Laura Bennett", today.AddDays(-15), "Specials summary drafted: waiting on kyphoplasty billing ledger."),
            N(case5, "Sarah Mitchell", today.AddDays(-3), "Notice of claim deadline is short. Escalate filing decision this week."),
            N(case6, "James Okafor", today.AddDays(-60), "Settled at mediation. Negotiating lien reductions with providers.")
        };

        db.Clients.AddRange(marcus, priya, daniel, hannah, tomasz, alicia);
        db.Providers.AddRange(voss, kessler, lakeshore, summit, harbor, cornerstone);
        db.Cases.AddRange(case1, case2, case3, case4, case5, case6);
        db.Treatments.AddRange(treatments);
        db.RecordsRequests.AddRange(requests);
        db.CaseNotes.AddRange(notes);
        db.SaveChanges();
    }

    private static Treatment T(LegalCase legalCase, Provider provider, DateTime date, string procedure,
        decimal billed, decimal paid, bool lien) => new()
    {
        LegalCase = legalCase,
        Provider = provider,
        VisitDate = date,
        Procedure = procedure,
        BilledAmount = billed,
        PaidAmount = paid,
        HasLien = lien
    };

    private static RecordsRequest R(LegalCase legalCase, Provider provider, DateTime requested, RequestStatus status,
        DateTime? followUp, DateTime? received, string notes) => new()
    {
        LegalCase = legalCase,
        Provider = provider,
        RequestedDate = requested,
        Status = status,
        FollowUpDate = followUp,
        ReceivedDate = received,
        Notes = notes
    };

    private static CaseNote N(LegalCase legalCase, string author, DateTime createdAt, string text) => new()
    {
        LegalCase = legalCase,
        Author = author,
        CreatedAt = createdAt,
        Text = text
    };
}
