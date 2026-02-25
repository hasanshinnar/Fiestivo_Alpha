document.addEventListener('DOMContentLoaded', function () {
    // 1. إضافة وظيفة التحكم في خصوصية الحدث
    const categorySelect = document.getElementById('Category_ID');
    const privacyToggle = document.getElementById('privacyToggle');
    const publicCategories = ['2', '3', '4', '5', '6', '7'];

    function updatePrivacyToggle() {
        const selectedValue = categorySelect.value;

        if (publicCategories.includes(selectedValue)) {
            // التصنيف العام: السويتش قابل للتغيير
            privacyToggle.disabled = false;
            privacyToggle.checked = true;
        } else {
            // تصنيف خاص: إجباري Private
            privacyToggle.checked = false;
            privacyToggle.disabled = true;
        }
    }

    // تحديث عند تغيير التصنيف
    categorySelect.addEventListener('change', updatePrivacyToggle);

    // تنفيذ عند التحميل في حالة وجود تصنيف مسبق
    updatePrivacyToggle();

    // 2. AI Modal functionality (الكود الأصلي مع تعديلات طفيفة)
    const aiModal = document.getElementById('aiModal');
    const openAiButton = document.getElementById('openAiModal');
    const addCategoryButton = document.getElementById('addCategory');
    const categoryContainer = document.getElementById('categoryContainer');

    // Create new category row
    function createCategoryRow() {
        const row = document.createElement('div');
        row.className = 'Create_category-row'; // تعديل ليتوافق مع كلاس CSS الصحيح
        row.innerHTML = `
            <select class="Create_category-row__select Create_form__select">
                <option value="Food">Food</option>
                <option value="Decoration">Decoration</option>
                <option value="Photo Booth">Photo Booth</option>
                <option value="DJ">DJ</option>
                <option value="Others">Others</option>
            </select>
            <input type="number" class="Create_category-row__input Create_form__input" placeholder="Amount">
            <button class="Create_category-row__remove-btn" onclick="removeCategoryRow(this)">×</button>
        `;
        return row;
    }

    // Remove category row
    window.removeCategoryRow = function (btn) {
        btn.closest('.Create_category-row').remove();
    }

    // Add category row
    addCategoryButton.addEventListener('click', () => {
        categoryContainer.appendChild(createCategoryRow());
    });

    // Open AI modal
    openAiButton.addEventListener('click', () => {
        aiModal.style.display = 'flex';
    });

    // Results modal functionality
    const resultsModal = document.getElementById('resultsModal');
    const closeResultsBtn = document.getElementById('closeResults');
    const saveRecommendationBtn = document.getElementById('saveRecommendation');

    // Show recommendations
    function showRecommendations() {
        // Dummy data - replace with actual AI results
        const recommendations = [
            {
                category: 'Food',
                userAmount: 600,
                recommendedAmount: 500,
                percentage: 50,
                tip: 'Consider reducing food budget and allocate more to decorations'
            },
            {
                category: 'Decoration',
                userAmount: 200,
                recommendedAmount: 250,
                percentage: 25,
                tip: 'Increase decoration budget for better visual impact'
            },
            {
                category: 'Photo Booth',
                userAmount: 100,
                recommendedAmount: 150,
                percentage: 15,
                tip: 'Photo booth budget should be increased for better quality equipment'
            },
            {
                category: 'DJ',
                userAmount: 100,
                recommendedAmount: 100,
                percentage: 10,
                tip: 'DJ budget allocation is optimal'
            }
        ];

        const resultsBody = document.getElementById('recommendationResults');
        resultsBody.innerHTML = recommendations.map(rec => `
            <tr>
                <td>${rec.category}</td>
                <td>${rec.userAmount} D</td>
                <td>${rec.recommendedAmount} D</td>
                <td>${rec.percentage}%</td>
            </tr>
        `).join('');

        resultsModal.style.display = 'flex';
    }

    // Get recommendation handler
    document.getElementById('getRecommendation').addEventListener('click', function () {
        const totalBudget = document.getElementById('totalBudget').value;
        const categories = Array.from(document.getElementsByClassName('Create_category-row__select')).map(select => select.value);
        const amounts = Array.from(document.getElementsByClassName('Create_category-row__input')).map(input => input.value);

        // Add actual AI recommendation logic here
        showRecommendations();
    });

    // Close results modal
    closeResultsBtn.addEventListener('click', () => {
        resultsModal.style.display = 'none';
    });

    // Save recommendation handler
    saveRecommendationBtn.addEventListener('click', function () {
        // Add save logic here
        alert('Recommendations saved successfully!');
        resultsModal.style.display = 'none';
        aiModal.style.display = 'none';
    });

    // Close modals on outside click
    window.addEventListener('click', (event) => {
        if (event.target === aiModal || event.target === resultsModal) {
            aiModal.style.display = 'none';
            resultsModal.style.display = 'none';
        }
    });
});