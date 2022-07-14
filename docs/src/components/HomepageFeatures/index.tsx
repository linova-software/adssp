import React from 'react';
import clsx from 'clsx';
import styles from './styles.module.css';

type FeatureItem = {
  title: string;
  imgSrc: string;
  description: JSX.Element;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'Change Password',
    imgSrc: require('@site/static/img/screenshot-change-pw.png').default,
    description: (
      <>
        Your Active Directory users may receive an initial password or may
        not have access to a domain computer to change their password. This
        feature enables them to change their password via a simple web interface.
      </>
    ),
  },
  {
    title: 'Test Password',
    imgSrc: require('@site/static/img/screenshot-test-pw.png').default,
    description: (
      <>
        Your Active Directory users may experience problems logging onto a
        system using their Active Directory credentials. With this feature
        they can easily test if their password is correct.
      </>
    ),
  },
  {
    title: 'Test Single Sign-on',
    imgSrc: require('@site/static/img/screenshot-auth-check.png').default,
    description: (
      <>
        Many companies use Single Sign-On using Negotiate Authentication in
        their web applications. This feature allows your users to test if the
        authentication feature works correctly when they experience problems
        with authentication in another web application.
      </>
    ),
  },
];

function Feature({ title, imgSrc, description }: FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className="text--center">
        <img src={imgSrc} className={styles.featureSvg} role="img" />
      </div>
      <div className="text--center padding-horiz--md">
        <h3>{title}</h3>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): JSX.Element {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
