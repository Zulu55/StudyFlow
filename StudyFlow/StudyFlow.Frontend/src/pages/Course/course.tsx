import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import './course.css';
import { useTheme } from '../../ThemeContext';
import NavBarCourse from '@components/NavBarCourse/NavBarCourse'; // Aseg�rate de que la ruta sea correcta
import Course_Announces from './Course_Announces/Course_Announces'; // Aseg�rate de que la ruta sea correcta
import Course_Classwork from './Course_Classwork/Course_Classwork'; // Aseg�rate de que la ruta sea correcta
import Course_People from './Course_People/Course_People'; // Aseg�rate de que la ruta sea correcta

const Course: React.FC = () => {
    const { t } = useTranslation();
    const { theme } = useTheme();

    // Estado para controlar qu� secci�n est� activa
    const [activeSection, setActiveSection] = useState<string>('announcements');

    const renderSection = () => {
        switch (activeSection) {
            case 'announcements':
                return <Course_Announces />;
            case 'classwork':
                return <Course_Classwork />;
            case 'people':
                return <Course_People />;
            default:
                return <Course_Announces />;
        }
    };

    return (
        <div className={`course-page ${theme}`}>

            <NavBarCourse setActiveSection={setActiveSection} />
            <div className="course-container">
                <div className="course-header">
                    <h1>2024-II G2 Software Construction</h1>
                    <h2>Software Engineering Specialization</h2>
                </div>

                <div className="course-layout">
                    {renderSection()}
                </div>
            </div>
        </div>
    );
};

export default Course;