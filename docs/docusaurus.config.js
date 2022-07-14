// @ts-check
// Note: type annotations allow type checking and IDEs autocompletion

const lightCodeTheme = require('prism-react-renderer/themes/github');
const darkCodeTheme = require('prism-react-renderer/themes/dracula');

/** @type {import('@docusaurus/types').Config} */
const config = {
    title: 'Active Directory Self-Service Portal',
    tagline: 'Fast and easy way for your users to change their password and test authentication',
    url: 'https://linova-software.github.io/',
    baseUrl: '/adssp/',
    onBrokenLinks: 'throw',
    onBrokenMarkdownLinks: 'warn',
    favicon: "/img/favicon-64px.png",
    organizationName: 'linova-software',
    projectName: 'linova-software.github.io',
    deploymentBranch: 'main',
    presets: [
        [
            'classic',
            /** @type {import('@docusaurus/preset-classic').Options} */
            ({
                docs: {
                    sidebarPath: require.resolve('./sidebars.js'),
                    editUrl: 'https://github.com/linova-software/adssp/tree/master/docs/',
                },
                blog: false,
                gtag: false,
                googleAnalytics: false,
                theme: {
                    customCss: require.resolve('./src/css/custom.css'),
                },
            }),
        ],
    ],

    themeConfig:
    /** @type {import('@docusaurus/preset-classic').ThemeConfig} */
        ({
        navbar: {
            title: 'Active Directory Self-Service Portal',
            logo: {
                alt: 'Logo of ADSSP',
                src: '/img/logo.svg',
            },
            items: [{
                    type: 'doc',
                    docId: 'intro',
                    position: 'left',
                    label: 'Documentation',
                },
                {
                    href: 'https://github.com/linova-software/adssp',
                    label: 'GitHub',
                    position: 'right',
                }
            ],
        },
        footer: {
            style: 'dark',
            links: [{
                    title: 'Company',
                    items: [{
                        label: 'Visit Linova on GitHub',
                        href: 'https://github.com/linova-software',
                    }, {
                        label: 'Carreer Opportunities',
                        href: 'https://www.linova.de/en/jobs',
                    }],
                },
                {
                    title: 'Legal',
                    items: [{
                            label: 'Imprint',
                            href: 'https://www.linova.de/en/impressum',
                        },
                        {
                            label: 'Privacy',
                            href: 'https://www.linova.de/en/datenschutz',
                        }
                    ],
                },
            ],
            copyright: `Copyright © ${new Date().getFullYear()} Linova Software GmbH`,
        },
        prism: {
            theme: lightCodeTheme,
            darkTheme: darkCodeTheme,
        },
        docs: {
            sidebar: {
                autoCollapseCategories: true,
            },
        },
    })
};

module.exports = config;