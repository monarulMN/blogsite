document.addEventListener('DOMContentLoaded', function () {

    // Tag button functionality
    document.querySelectorAll('.tag-button').forEach(button => {
        button.addEventListener('click', function () {
            document.querySelectorAll('.tag-button')
                .forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');
        });
    });

    // Follow button functionality
    document.querySelectorAll('.btn-follow-small').forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.stopPropagation();
            this.classList.toggle('visiting');
            this.textContent = this.classList.contains('visiting')
                ? 'visiting'
                : 'visit';
        });
    });

    // FAB button (SAFE)
    const fab = document.querySelector('.fab-write');
    if (fab) {
        fab.addEventListener('click', function () {
            alert('Opening blog editor...');
        });
    }

    // User dropdown
    const avatar = document.getElementById('userAvatar');
    const dropdown = document.getElementById('userDropdown');

    if (!avatar || !dropdown) return;

    avatar.addEventListener('click', function (e) {
        e.stopPropagation();
        dropdown.style.display =
            dropdown.style.display === 'block' ? 'none' : 'block';
    });

    dropdown.addEventListener('click', function (e) {
        e.stopPropagation();
    });

    document.addEventListener('click', function () {
        dropdown.style.display = 'none';
    });
});