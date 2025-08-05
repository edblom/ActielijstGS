const { test, expect } = require('@playwright/test');

test.describe('ActielijstAPI Endpoints', () => {
    let request;

    test.beforeAll(async ({ playwright }) => {
        // Maak een aangepaste request-context met certificaatvalidatie uitgeschakeld
        request = await playwright.request.newContext({
            baseURL: 'https://localhost:44361/api',
            ignoreHTTPSErrors: true,
            extraHTTPHeaders: {
                'Accept': 'application/json',
            },
        });
    });

    test.afterAll(async () => {
        await request.dispose();
    });

    // Test /api/acties (GET)
    test('GET /api/acties retourneert een lijst', async () => {
        const response = await request.get('/acties');
        expect(response.status()).toBe(200);
        const body = await response.json();
        expect(Array.isArray(body)).toBe(true); // Verwacht een array
    });

    // Test /api/acties (POST)
    test('POST /api/acties creëert een nieuwe actie', async () => {
        const newActie = {
            FldMid: 999,
            FldMDatum: '2025-08-05T21:00:00Z',
            WerknId: 1,
            // Vul andere velden naar behoefte (bijv. FldMKlantId, FldOmschrijving)
        };
        const response = await request.post('/acties', {
            data: newActie,
        });
        expect(response.status()).toBe(200);
        const body = await response.json();
        expect(body.FldMid).toBe(999); // Controleer de geretourneerde actie
    });

    // Test /api/acties/{id} (GET)
    test('GET /api/acties/{id} retourneert een actie', async () => {
        const response = await request.get('/acties/1'); // Pas ID aan naar een bestaande actie
        expect(response.status()).toBe(200);
        const body = await response.json();
        expect(body).toHaveProperty('FldMid'); // Controleer aanwezigheid van veld
    });

    // Test /api/acties/{id} (PATCH)
    test('PATCH /api/acties/{id} wijzigt een actie', async () => {
        const patchData = {
            fldMActieGereed: '2025-08-05T21:30:00Z',
        };
        const response = await request.patch('/acties/1', { // Pas ID aan
            data: patchData,
        });
        expect(response.status()).toBe(200);
        // Optioneel: Haal de actie op om te valideren
    });

    // Test /api/documents/generate/{inspectieId} (POST)
    test('POST /api/documents/generate/{inspectieId} genereert een document', async () => {
        const response = await request.post('/documents/generate/1', { // Pas inspectieId aan
            data: { soort: 1, openDocumentInWord: true },
        });
        expect(response.status()).toBe(200);
        const body = await response.json();
        expect(body).toHaveProperty('correspondentieId');
    });

    // Voeg meer endpoints toe naar behoefte (bijv. /api/login, /api/priorities)
});