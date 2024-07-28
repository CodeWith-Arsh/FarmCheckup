document.getElementById('submitPhoto').addEventListener('click', async () => {
    const photoInput = document.getElementById('leafPhoto').files[0];
    if (!photoInput) {
        alert('Please select a photo first.');
        return;
    }

    const formData = new FormData();
    formData.append('photo', photoInput);

    try {
        const response = await fetch('/api/analyze', {
            method: 'POST',
            body: formData
        });
        const result = await response.json();
        document.getElementById('result').innerText = `Diagnosis: ${result.diagnosis}\nSolution: ${result.solution}`;
    } catch (error) {
        console.error('Error:', error);
        alert('An error occurred while analyzing the photo.');
    }
});
