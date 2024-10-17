export const menuItems = [
    {
        items: [
            { label: 'Map', icon: 'fa fa-earth-americas', to: '/' },
            { label: 'Favorites', icon: 'fa fa-heart', to: '/favorites' },
            { label: 'Events', icon: 'fa fa-calendar-days', to: '/events' },
            { label: 'Community', icon: 'fa fa-people-group', to: '/events' },
        ]
    },
    {
        icon: 'pi pi-fw pi-briefcase',
        to: '/pages',
        items: [
            {
                label: 'My Account',
                icon: 'fa fa-user',
                items: [
                    {
                        label: 'Login',
                        icon: 'pi pi-fw pi-sign-in',
                        to: '/auth/login'
                    },
                    {
                        label: 'Error',
                        icon: 'pi pi-fw pi-times-circle',
                        to: '/auth/error'
                    },
                    {
                        label: 'Access Denied',
                        icon: 'pi pi-fw pi-lock',
                        to: '/auth/access'
                    }
                ]
            }
        ]
    },
];